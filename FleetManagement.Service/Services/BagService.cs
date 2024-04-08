using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using FleetManagement.Core.Services;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Service.Constants;

namespace FleetManagement.Service.Services
{
    public class BagService : IBagService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBagRepository bagRepository;
        private readonly IDeliveryPointRepository deliveryPointRepository;

        public BagService(IUnitOfWork unitOfWork, IBagRepository bagRepository, IDeliveryPointRepository deliveryPointRepository)
        {
            this.unitOfWork = unitOfWork;
            this.bagRepository = bagRepository;
            this.deliveryPointRepository = deliveryPointRepository;
        }

        public async Task<Bag> GetByBarcodeAsync(string barcode)
        {
            var bag = await bagRepository.GetSingleBagByBarcodeAsync(barcode);
            if (bag is null)
                throw new KeyNotFoundException(string.Format(Messages.BagNotFound, barcode));

            return bag;
        }

        public async Task<Bag> AddAsync(Bag bag)
        {
            var isExist = await bagRepository.IsExistBag(bag.Barcode);
            if (isExist)
                throw new ApplicationException(string.Format(Messages.BagAlreadyExist, bag.Barcode));

            DeliveryPoint? releatedDeliveryPoint = null;
            if (bag.DeliveryPoint != null)
                releatedDeliveryPoint = await deliveryPointRepository.GetSingleDeliveryPointByValueAsync(bag.DeliveryPoint.Value);

            if (releatedDeliveryPoint is null)
                throw new KeyNotFoundException(string.Format(Messages.DeliveryPointNotFound, bag.DeliveryPoint?.Value));

            bag.DeliveryPointId = releatedDeliveryPoint.Id;

            await bagRepository.AddAsync(bag);
            await unitOfWork.CommitAsync();
            return bag;
        }
    }
}
