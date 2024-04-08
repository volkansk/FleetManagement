using AutoMapper;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Entities;
using FleetManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    [Produces("application/json")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleService vehicleService;

        public VehiclesController(IMapper mapper, IVehicleService vehicleService)
        {
            this.mapper = mapper;
            this.vehicleService = vehicleService;
        }

        /// <summary>
        /// Create a Vehicle
        /// </summary>
        /// <param name="vehicleDto"></param>
        /// <returns></returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /vehicles
        ///     {
        ///        "plate": "34 TL 34"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(VehicleResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync(VehicleDto vehicleDto)
        {
            var vehicle = await vehicleService.AddAsync(mapper.Map<Vehicle>(vehicleDto));

            var vehicleResultDto = mapper.Map<VehicleResultDto>(vehicle);

            return new JsonResult(vehicleResultDto);
        }
    }
}
