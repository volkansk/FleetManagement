using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Repositories
{
    public interface IPackageRepository
    {
        Task AddAsync(Package package);
        void Update(Package package);
        Task<Package> GetSinglePackageByBarcodeAsync(string barcode);
        Task<bool> IsExistsPackage(string barcode);
    }
}
