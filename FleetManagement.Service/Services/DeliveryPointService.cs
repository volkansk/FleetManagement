using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using FleetManagement.Core.Services;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Service.Constants;

namespace FleetManagement.Service.Services
{
    public class DeliveryPointService : IDeliveryPointService
    {
        private readonly IDeliveryPointRepository deliveryPointRepository;
        private readonly IUnitOfWork unitOfWork;
        public DeliveryPointService(IDeliveryPointRepository deliveryPointRepository, IUnitOfWork unitOfWork)
        {
            this.deliveryPointRepository = deliveryPointRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<DeliveryPoint> AddAsync(DeliveryPoint deliveryPoint)
        {
            var isExist = await deliveryPointRepository.IsExistsDeliveryPoint(deliveryPoint.Value);
            if (isExist)
                throw new ApplicationException(string.Format(Messages.DeliveryPointAlreadyExist, deliveryPoint.Value));

            await deliveryPointRepository.AddAsync(deliveryPoint);
            await unitOfWork.CommitAsync();
            return deliveryPoint;
        }

        public async Task<DeliveryPoint> GetByValueAsync(int value)
        {
            var deliveryPoint = await deliveryPointRepository.GetSingleDeliveryPointByValueAsync(value);
            if (deliveryPoint is null)
                throw new KeyNotFoundException(string.Format(Messages.DeliveryPointNotFound, value));

            return deliveryPoint;
        }
    }
}
