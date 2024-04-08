using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Repository.Repositories
{
    public class BagRepository : IBagRepository
    {
        protected readonly DataContext _context;
        private readonly DbSet<Bag> _dbSet;

        public BagRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<Bag>();
        }

        public async Task AddAsync(Bag bag)
        {
            await _dbSet.AddAsync(bag);
        }

        public async Task<Bag> GetSingleBagWithPackagesByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Packages).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Bag> GetSingleBagByBarcodeAsync(string barcode)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Barcode == barcode);
        }

        public async Task<Bag> GetSingleBagWithPackagesByBarcodeAsync(string barcode)
        {
            return await _dbSet.Include(x => x.Packages).SingleOrDefaultAsync(x => x.Barcode == barcode);
        }

        public async Task<bool> IsExistBag(string barcode)
        {
            return await _dbSet.AnyAsync(x => x.Barcode == barcode);
        }
    }
}
