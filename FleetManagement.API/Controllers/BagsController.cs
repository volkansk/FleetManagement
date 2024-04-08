using AutoMapper;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Entities;
using FleetManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/bags")]
    [ApiController]
    [Produces("application/json")]
    public class BagsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBagService bagService;

        public BagsController(IMapper mapper, IBagService bagService)
        {
            this.mapper = mapper;
            this.bagService = bagService;
        }

        /// <summary>
        /// Get Bag By Barcode 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [HttpGet("{barcode}")]
        [ProducesResponseType(typeof(BagResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByBarcodeAsync(string barcode)
        {
            var bag = await bagService.GetByBarcodeAsync(barcode);

            var bagResultDto = mapper.Map<BagResultDto>(bag);

            return new JsonResult(bagResultDto);
        }

        /// <summary>
        /// Create a Bag
        /// </summary>
        /// <param name="bagDto"></param>
        /// <returns>A newly created Bag</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /bags
        ///     {
        ///        "barcode": "C725799",
        ///        "deliveryPointValue": 2
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(BagResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAsync(BagDto bagDto)
        {
            var bag = await bagService.AddAsync(mapper.Map<Bag>(bagDto));

            var bagResultDto = mapper.Map<BagResultDto>(bag);

            return new JsonResult(bagResultDto);
        }
    }
}
