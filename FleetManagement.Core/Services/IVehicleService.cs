using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> AddAsync(Vehicle package);
    }
}
