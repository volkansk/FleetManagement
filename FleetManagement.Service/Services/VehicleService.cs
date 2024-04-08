using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using FleetManagement.Core.Services;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Service.Constants;

namespace FleetManagement.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        public VehicleService(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            this.vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            var isExist = await vehicleRepository.IsExistVehicle(vehicle.Plate);
            if (isExist)
                throw new ApplicationException(string.Format(Messages.VehicleAlreadyExist, vehicle.Plate));

            await vehicleRepository.AddAsync(vehicle);
            await unitOfWork.CommitAsync();
            return vehicle;
        }
    }
}
