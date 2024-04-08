using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Services
{
    public interface IBagService
    {
        Task<Bag> GetByBarcodeAsync(string barcode);
        Task<Bag> AddAsync(Bag bag);
    }
}
