using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Repository.Repositories
{
    public class FleetTransactionRepository : IFleetTransactionRepository
    {
        protected readonly DataContext _context;
        private readonly DbSet<FleetTransaction> _dbSet;

        public FleetTransactionRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<FleetTransaction>();
        }

        public async Task AddAsync(FleetTransaction fleetTransaction)
        {
            await _dbSet.AddAsync(fleetTransaction);
        }
    }
}
