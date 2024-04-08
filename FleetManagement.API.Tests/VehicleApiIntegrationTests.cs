using FleetManagement.API.Tests.Utilities;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Service.Constants;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.API.Tests
{
    public class VehicleApiIntegrationTests : IntegrationTest
    {
        #region Return Success

        [Theory]
        [InlineData("34 KS 34", "34 KS 34")]
        [InlineData("34 VSI 34", "34 VSI 34")]
        [InlineData("34 VSI 3400", "34 VSI 3400")]
        [InlineData("VSI 34 8911", "VSI 34 8911")]
        public async Task AddVehicle_ShouldBeAdded_WhenGivenValidPlate_ReturnSuccess(string plate, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Vehicle.AddSync, new VehicleDto { plate = plate });
            var vehicleResultDto = await response.Content.ReadFromJsonAsync<VehicleResultDto>();

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(expected, vehicleResultDto?.plate);
        }

        #endregion

        #region Return Exception

        [Theory]
        [InlineData(null, "plate is required.")]
        [InlineData("", "plate is required.")]
        public async Task AddVehicle_ShouldNotBeAdded_WhenGivenNullOrEmptyPlate_ReturnRequiredException(string plate, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Vehicle.AddSync, new VehicleDto { plate = plate });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("VSI 34 89112", "Maximum length of plate is 11")]
        public async Task AddVehicle_ShouldNotBeAdded_WhenGiven12LengthPlate_ReturnMaximumLengthException(string plate, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Vehicle.AddSync, new VehicleDto { plate = plate });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("VSI 34 8913")]
        public async Task AddVehicle_ShouldNotBeAdded_WhenGivenDuplicatePlate_ReturnAlreadyExistException(string plate)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Vehicle.AddSync, new VehicleDto { plate = plate });
            var responseDuplicated = await TestClient.PostAsJsonAsync(ApiRoutes.Vehicle.AddSync, new VehicleDto { plate = plate });
            var errorResponseDto = await responseDuplicated.Content.ReadFromJsonAsync<ErrorResponseDto>();

            response.EnsureSuccessStatusCode();
            Assert.False(responseDuplicated.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.VehicleAlreadyExist, plate), errorResponseDto?.Error);
        }

        #endregion
    }
}
