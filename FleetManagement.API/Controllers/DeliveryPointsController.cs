using AutoMapper;
using FleetManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.Entities;
using FleetManagement.Core.DTOs.Output;

namespace FleetManagement.API.Controllers
{
    [Route("api/deliverypoints")]
    [ApiController]
    [Produces("application/json")]
    public class DeliveryPointsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDeliveryPointService deliveryPointService;

        public DeliveryPointsController(IMapper mapper, IDeliveryPointService deliveryPointService)
        {
            this.mapper = mapper;
            this.deliveryPointService = deliveryPointService;
        }

        /// <summary>
        /// Get Delivery Point By Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("{value}")]
        [ProducesResponseType(typeof(DeliveryPointResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByValueAsync(int value)
        {
            var deliveryPoint = await deliveryPointService.GetByValueAsync(value);

            var deliveryPointResultDto = mapper.Map<DeliveryPointResultDto>(deliveryPoint);

            return new JsonResult(deliveryPointResultDto);
        }

        /// <summary>
        /// Create a Delivery Point
        /// </summary>
        /// <param name="deliveryPointDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /deliverypoints
        ///     {
        ///        "type": 1,
        ///        "value": 1
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(DeliveryPointResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync(DeliveryPointDto deliveryPointDto)
        {
            var deliveryPoint = await deliveryPointService.AddAsync(mapper.Map<DeliveryPoint>(deliveryPointDto));

            var deliveryPointResultDto = mapper.Map<DeliveryPointResultDto>(deliveryPoint);

            return new JsonResult(deliveryPointResultDto);
        }
    }
}
