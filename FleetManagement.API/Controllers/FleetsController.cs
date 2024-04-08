using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/fleets")]
    [ApiController]
    [Produces("application/json")]
    public class FleetsController : ControllerBase
    {
        private readonly IDistributionService distributionService;

        public FleetsController(IDistributionService distributionService)
        {
            this.distributionService = distributionService;
        }

        /// <summary>
        /// Used to deliver packages to vehicle-related delivery points.
        /// </summary>
        /// <param name="distributionDto"></param>
        /// <returns>DistributionResultDto</returns>
        [Route("distribute")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Distribute(DistributionCommandDto distributionDto)
        {
            DistributionResultDto distributionResultDto = await distributionService.Distribute(distributionDto);

            return new JsonResult(distributionResultDto);
        }
    }
}
