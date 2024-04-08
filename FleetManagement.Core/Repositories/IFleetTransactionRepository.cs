using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Repositories
{
    public interface IFleetTransactionRepository
    {
        Task AddAsync(FleetTransaction entity);
    }
}
