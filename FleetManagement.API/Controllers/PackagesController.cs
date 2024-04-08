using AutoMapper;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Entities;
using FleetManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/packages")]
    [ApiController]
    [Produces("application/json")]
    public class PackagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPackageService packageService;

        public PackagesController(IMapper mapper, IPackageService packageService)
        {
            this.mapper = mapper;
            this.packageService = packageService;
        }

        /// <summary>
        /// Get Package By Barcode
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [HttpGet("{barcode}")]
        [ProducesResponseType(typeof(PackageResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByBarcodeAsync(string barcode)
        {
            var package = await packageService.GetByBarcodeAsync(barcode);

            var packageResultDto = mapper.Map<PackageResultDto>(package);

            return new JsonResult(packageResultDto);
        }

        /// <summary>
        /// Create a Package
        /// </summary>
        /// <param name="packageDto"></param>
        /// <returns>A newly created Package</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /packages
        ///     {
        ///        "barcode": "P7988000122",
        ///        "deliveryPointValue": 1,
        ///        "volumetricWeight": 5
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(PackageResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAsync(PackageDto packageDto)
        {
            var package = await packageService.AddAsync(mapper.Map<Package>(packageDto));

            var packageResultDto = mapper.Map<PackageResultDto>(package);

            return new JsonResult(packageResultDto);
        }

        /// <summary>
        /// Used to assign packages to bags.
        /// </summary>
        /// <param name="assignSinglePackageDto"></param>
        /// <returns></returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /packages
        ///     {
        ///        "packageBarcode": "P8988000126",
        ///        "bagBarcode": "C725799"
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignSinglePackage(AssignSinglePackageDto assignSinglePackageDto)
        {
            await packageService.AssignPackageToBag(assignSinglePackageDto);

            return new JsonResult("OK");
        }
    }
}
