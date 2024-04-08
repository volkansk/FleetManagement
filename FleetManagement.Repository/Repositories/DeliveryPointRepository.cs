using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Repository.Repositories
{
    public class DeliveryPointRepository : IDeliveryPointRepository
    {
        protected readonly DataContext _context;
        private readonly DbSet<DeliveryPoint> _dbSet;

        public DeliveryPointRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<DeliveryPoint>();
        }

        public async Task AddAsync(DeliveryPoint deliveryPoint)
        {
            await _dbSet.AddAsync(deliveryPoint);
        }

        public async Task<bool> IsExistsDeliveryPoint(int value)
        {
            return await _dbSet.AnyAsync(x => x.Value == value);
        }

        public async Task<DeliveryPoint> GetSingleDeliveryPointByValueAsync(int value)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Value == value);
        }
    }
}
