using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.Entities;

namespace FleetManagement.Core.Services
{
    public interface IPackageService
    {
        Task<Package> AddAsync(Package package);

        /// <summary>
        /// Used to assign packages to bags.
        /// </summary>
        /// <param name="assignSinglePackageDto"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ApplicationException"></exception>
        Task AssignPackageToBag(AssignSinglePackageDto assignSinglePackageDto);
        Task<Package> GetByBarcodeAsync(string barcode);
    }
}
