using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using FleetManagement.Core.Services;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Service.Constants;

namespace FleetManagement.Service.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository packageRepository;
        private readonly IBagRepository bagRepository;
        private readonly IDeliveryPointRepository deliveryPointRepository;
        private readonly IUnitOfWork unitOfWork;
        public PackageService(IPackageRepository packageRepository, IBagRepository bagRepository, IDeliveryPointRepository deliveryPointRepository, IUnitOfWork unitOfWork)
        {
            this.packageRepository = packageRepository;
            this.bagRepository = bagRepository;
            this.deliveryPointRepository = deliveryPointRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Package> GetByBarcodeAsync(string barcode)
        {
            var package = await packageRepository.GetSinglePackageByBarcodeAsync(barcode);
            if (package is null)
                throw new KeyNotFoundException(string.Format(Messages.PackageNotFound, barcode));

            return package;
        }

        public async Task<Package> AddAsync(Package package)
        {
            var isExist = await packageRepository.IsExistsPackage(package.Barcode);
            if (isExist)
                throw new ApplicationException(string.Format(Messages.PackageAlreadyExist, package.Barcode));

            DeliveryPoint? releatedDeliveryPoint = null;
            if (package.DeliveryPoint != null)
                releatedDeliveryPoint = await deliveryPointRepository.GetSingleDeliveryPointByValueAsync(package.DeliveryPoint.Value);

            if (releatedDeliveryPoint is null)
                throw new KeyNotFoundException(string.Format(Messages.DeliveryPointNotFound, package.DeliveryPoint?.Value));

            package.DeliveryPointId = releatedDeliveryPoint.Id;

            await packageRepository.AddAsync(package);
            await unitOfWork.CommitAsync();
            return package;
        }

        public async Task AssignPackageToBag(AssignSinglePackageDto assignSinglePackageDto)
        {
            var bag = await bagRepository.GetSingleBagByBarcodeAsync(assignSinglePackageDto.BagBarcode);
            if (bag is null)
                throw new KeyNotFoundException(string.Format(Messages.BagNotFound, assignSinglePackageDto.BagBarcode));


            var package = await packageRepository.GetSinglePackageByBarcodeAsync(assignSinglePackageDto.PackageBarcode);
            if (package is null)
                throw new KeyNotFoundException(string.Format(Messages.PackageNotFound, assignSinglePackageDto.PackageBarcode));


            if (package.DeliveryPointId != bag.DeliveryPointId)
                throw new ApplicationException(Messages.NotSamePackageBagDeliveryPoint);


            package.BagId = bag.Id;
            package.State = Core.Enumerations.PackageStatuses.LoadedIntoBag;

            packageRepository.Update(package);
            await unitOfWork.CommitAsync();
        }
    }
}
