using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Repositories
{
    public interface IBagRepository
    {
        Task AddAsync(Bag entity);
        Task<Bag> GetSingleBagByBarcodeAsync(string barcode);
        Task<Bag> GetSingleBagWithPackagesByIdAsync(int id);
        Task<Bag> GetSingleBagWithPackagesByBarcodeAsync(string barcode);
        Task<bool> IsExistBag(string barcode);
    }
}
