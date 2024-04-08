using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Services
{
    public interface IDeliveryPointService
    {
        Task<DeliveryPoint> AddAsync(DeliveryPoint deliveryPoint);
        Task<DeliveryPoint> GetByValueAsync(int value);
    }
}
