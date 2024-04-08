using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Repositories
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle entity);
        Task<bool> IsExistVehicle(string plate);
        Task<Vehicle> GetSingleVehicleByPlateAsync(string plate);
    }
}
