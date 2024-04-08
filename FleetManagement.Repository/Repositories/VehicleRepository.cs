using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Repository.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        protected readonly DataContext _context;
        private readonly DbSet<Vehicle> _dbSet;

        public VehicleRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<Vehicle>();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _dbSet.AddAsync(vehicle);
        }

        public async Task<bool> IsExistVehicle(string plate)
        {
            return await _dbSet.AnyAsync(x => x.Plate == plate);
        }

        public async Task<Vehicle> GetSingleVehicleByPlateAsync(string plate)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Plate == plate);
        }
    }
}
