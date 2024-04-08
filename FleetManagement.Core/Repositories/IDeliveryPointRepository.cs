using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Repositories
{
    public interface IDeliveryPointRepository
    {
        Task AddAsync(DeliveryPoint entity);
        Task<bool> IsExistsDeliveryPoint(int value);
        Task<DeliveryPoint> GetSingleDeliveryPointByValueAsync(int value);
    }
}
