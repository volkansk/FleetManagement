using FleetManagement.Core.Entities;
using FleetManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Repository.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        protected readonly DataContext _context;
        private readonly DbSet<Package> _dbSet;

        public PackageRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<Package>();
        }

        public async Task AddAsync(Package package)
        {
            await _dbSet.AddAsync(package);
        }

        public async Task<bool> IsExistsPackage(string barcode)
        {
            return await _dbSet.AnyAsync(x => x.Barcode == barcode);
        }

        public async Task<Package> GetSinglePackageByBarcodeAsync(string barcode)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Barcode == barcode);
        }

        public void Update(Package package)
        {
            package.UpdatedDate = DateTime.Now;
            _dbSet.Update(package);
        }
    }
}
