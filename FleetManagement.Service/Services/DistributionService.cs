using AutoMapper;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Entities;
using FleetManagement.Core.Enumerations;
using FleetManagement.Core.Repositories;
using FleetManagement.Core.Services;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Service.Constants;

namespace FleetManagement.Service.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly IFleetTransactionRepository fleetTransactionRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IDeliveryPointRepository deliveryPointRepository;
        private readonly IPackageRepository packageRepository;
        private readonly IBagRepository bagRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        private readonly Guid transactionId;
        private readonly List<int> packageDeliveredBagIds;
        private DistributionResultDto distributionResultDto;
        private int vehicleId;

        public DistributionService(IFleetTransactionRepository fleetTransactionRepository, IVehicleRepository vehicleRepository, IDeliveryPointRepository deliveryPointRepository, IPackageRepository packageRepository, IBagRepository bagRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.fleetTransactionRepository = fleetTransactionRepository;
            this.vehicleRepository = vehicleRepository;
            this.deliveryPointRepository = deliveryPointRepository;
            this.packageRepository = packageRepository;
            this.bagRepository = bagRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;

            transactionId = Guid.NewGuid();
            distributionResultDto = new DistributionResultDto();
            packageDeliveredBagIds = new();
        }

        public async Task<DistributionResultDto> Distribute(DistributionCommandDto distributionCommand)
        {
            this.distributionResultDto = mapper.Map<DistributionResultDto>(distributionCommand);
            await CheckVehicleAsync(distributionCommand.plate);

            foreach (var route in distributionResultDto.route)
            {
                DeliveryPoint? deliveryPoint = await GetDeliveryPointAsync(route.deliveryPoint);
                if (deliveryPoint == null) continue;

                await ProcessDeliveriesAsync(route.deliveries, deliveryPoint);
            }

            await CheckPackagesIntoBagStateAsync();

            await unitOfWork.CommitAsync();
            return distributionResultDto;
        }

        #region Private Methods

        private async Task CheckVehicleAsync(string plate)
        {
            var vehicle = await vehicleRepository.GetSingleVehicleByPlateAsync(plate);
            if (vehicle is null)
                throw new KeyNotFoundException(string.Format(Messages.VehicleNotFound, plate));

            this.vehicleId = vehicle.Id;
        }

        private async Task<DeliveryPoint?> GetDeliveryPointAsync(int deliveryPointValue)
        {
            var deliveryPoint = await deliveryPointRepository.GetSingleDeliveryPointByValueAsync(deliveryPointValue);
            if (deliveryPoint is null)
                AddSyncFleetTransaction(new FleetTransaction { Message = string.Format(Messages.DeliveryPointNotFound, deliveryPointValue), State = 0 });

            return deliveryPoint;
        }

        private async Task ProcessDeliveriesAsync(List<DistributionResultDto.Route.Delivery> deliveries, DeliveryPoint deliveryPoint)
        {
            foreach (var delivery in deliveries)
            {
                Package package = await packageRepository.GetSinglePackageByBarcodeAsync(delivery.barcode);
                if (package != null)
                {
                    DistributePackage(package, delivery, deliveryPoint);
                }
                else
                {
                    Bag bag = await bagRepository.GetSingleBagWithPackagesByBarcodeAsync(delivery.barcode);
                    if (bag == null)
                    {
                        AddSyncFleetTransaction(new FleetTransaction { Barcode = delivery.barcode, DeliveryPointId = deliveryPoint.Id, State = (int)PackageStatuses.Loaded, Message = string.Format(Messages.BarcodeNotFound, delivery.barcode) });
                        continue;
                    }

                    DistributeBag(bag, delivery, deliveryPoint);
                }
            }
        }

        private void DistributePackage(Package package, DistributionResultDto.Route.Delivery delivery, DeliveryPoint deliveryPoint)
        {
            ChangeStates(package, delivery, PackageStatuses.Loaded);

            if (package.DeliveryPointId != deliveryPoint.Id)
            {
                AddSyncFleetTransaction(new FleetTransaction { Barcode = package.Barcode, DeliveryPointId = deliveryPoint.Id, State = (int)PackageStatuses.Loaded, Message = Messages.WrongDeliveryPoint });
                return;
            }

            var isUnloaded = CanPackageBeUnloaded(deliveryPoint, package);
            if (isUnloaded)
            {
                ChangeStates(package, delivery, PackageStatuses.Unloaded);
                AddSyncFleetTransaction(new FleetTransaction { Barcode = delivery.barcode, DeliveryPointId = deliveryPoint.Id, State = (int)PackageStatuses.Unloaded, Message = Messages.PackageUnloaded });
            }
        }

        private void DistributeBag(Bag bag, DistributionResultDto.Route.Delivery delivery, DeliveryPoint deliveryPoint)
        {
            ChangeStates(bag, delivery, BagStatuses.Loaded);

            if (bag.DeliveryPointId != deliveryPoint.Id)
            {
                AddSyncFleetTransaction(new FleetTransaction { Barcode = bag.Barcode, DeliveryPointId = deliveryPoint.Id, State = (int)BagStatuses.Loaded, Message = Messages.WrongDeliveryPoint });
                return;
            }

            if (deliveryPoint.Type != DeliveryPointTypes.Branch)
            {
                ChangeStates(bag, delivery, BagStatuses.Unloaded);
                UnloadPackagesIntoBag(bag);

                AddSyncFleetTransaction(new FleetTransaction { Barcode = delivery.barcode, DeliveryPointId = deliveryPoint.Id, State = (int)BagStatuses.Unloaded, Message = Messages.BagUnloaded });
            }
        }

        private bool CanPackageBeUnloaded(DeliveryPoint deliveryPoint, Package package)
        {
            bool unloaded = false;
            switch (deliveryPoint.Type)
            {
                case DeliveryPointTypes.Branch:
                    if (package.BagId == null)
                        unloaded = true;
                    break;
                case DeliveryPointTypes.DistributionCenter:
                    unloaded = true;
                    break;
                case DeliveryPointTypes.TransferCenter:
                    if (package.BagId != null)
                    {
                        unloaded = true;
                        if (!packageDeliveredBagIds.Contains(package.BagId.Value)) packageDeliveredBagIds.Add(package.BagId.Value);
                    }
                    break;
            }
            return unloaded;
        }

        private void UnloadPackagesIntoBag(Bag bag)
        {
            var loadedPackagesInBag = bag.Packages.Where(x => x.State != PackageStatuses.Unloaded);

            if (loadedPackagesInBag != null && loadedPackagesInBag.Any())
                foreach (var loadedPackage in loadedPackagesInBag)
                {
                    loadedPackage.ChangeState(PackageStatuses.Unloaded);
                    AddSyncFleetTransaction(new FleetTransaction { Barcode = loadedPackage.Barcode, DeliveryPointId = loadedPackage.DeliveryPointId, State = (int)PackageStatuses.Unloaded, Message = Messages.PackageIntoBagUnloaded });
                }
        }

        private async Task CheckPackagesIntoBagStateAsync()
        {
            foreach (int bagId in packageDeliveredBagIds)
            {
                Bag bag = await bagRepository.GetSingleBagWithPackagesByIdAsync(bagId);
                if (!bag.Packages.Any(x => x.State != PackageStatuses.Unloaded))
                {
                    bag.ChangeState(BagStatuses.Unloaded);
                    AddSyncFleetTransaction(new FleetTransaction { Barcode = bag.Barcode, DeliveryPointId = bag.DeliveryPointId, State = (int)PackageStatuses.Unloaded, Message = Messages.BagUnloadedAfterPackages });
                }
            }
        }

        private static void ChangeStates(Package package, DistributionResultDto.Route.Delivery delivery, PackageStatuses status)
        {
            package.ChangeState(status);
            delivery.ChangeState((int)status);
        }

        private static void ChangeStates(Bag bag, DistributionResultDto.Route.Delivery delivery, BagStatuses status)
        {
            bag.ChangeState(status);
            delivery.ChangeState((int)status);
        }

        private void AddSyncFleetTransaction(FleetTransaction fleetTransaction)
        {
            fleetTransaction.VehicleId = this.vehicleId;
            fleetTransaction.TransactionId = this.transactionId;
            _ = fleetTransactionRepository.AddAsync(fleetTransaction);
        }

        #endregion
    }
}
