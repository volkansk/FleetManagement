using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;

namespace FleetManagement.Core.Services
{
    public interface IDistributionService
    {
        /// <summary>
        /// Used to deliver packages to vehicle-related delivery points.
        /// </summary>
        /// <param name="distributionCommand"></param>
        /// <returns>DistributionResultDto</returns>
        Task<DistributionResultDto> Distribute(DistributionCommandDto distributionCommand);
    }
}
