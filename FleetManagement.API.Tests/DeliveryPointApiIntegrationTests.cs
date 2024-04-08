using FleetManagement.API.Tests.Utilities;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Core.Enumerations;
using FleetManagement.Service.Constants;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.API.Tests
{
    public class DeliveryPointApiIntegrationTests : IntegrationTest
    {
        #region Return Success

        [Theory]
        [InlineData(DeliveryPointTypes.Branch, 101, (int)DeliveryPointTypes.Branch, 101)]
        [InlineData(DeliveryPointTypes.DistributionCenter, 102, (int)DeliveryPointTypes.DistributionCenter, 102)]
        [InlineData(DeliveryPointTypes.TransferCenter, 103, (int)DeliveryPointTypes.TransferCenter, 103)]
        public async Task AddDeliveryPoint_ShouldBeAdded_WhenGivenValidDeliveryPoint_ReturnSuccess(DeliveryPointTypes type, int value, int expectedType, int expectedValue)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.DeliveryPoint.AddSync, new DeliveryPointDto { type = type, value = value });
            var deliveryPointResultDto = await response.Content.ReadFromJsonAsync<DeliveryPointResultDto>();

            Assert.NotNull(deliveryPointResultDto);
            Assert.Equal(expectedType, deliveryPointResultDto?.type);
            Assert.Equal(expectedValue, deliveryPointResultDto?.value);
        }

        #endregion

        #region Return Exception

        [Theory]
        [InlineData(DeliveryPointTypes.Branch, 0, "value must be greater 0.")]
        public async Task AddDeliveryPoint_ShouldNotBeAdded_WhenGivenNegativeDeliveryPointValue_ReturnGreaterException(DeliveryPointTypes type, int value, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.DeliveryPoint.AddSync, new DeliveryPointDto { type = type, value = value });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData(DeliveryPointTypes.Branch, 104)]
        [InlineData(DeliveryPointTypes.DistributionCenter, 105)]
        [InlineData(DeliveryPointTypes.TransferCenter, 106)]
        public async Task AddDeliveryPoint_ShouldNotBeAdded_WhenGivenDuplicateValue_ReturnAlreadyExistException(DeliveryPointTypes type, int value)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.DeliveryPoint.AddSync, new DeliveryPointDto { type = type, value = value });
            var responseDuplicated = await TestClient.PostAsJsonAsync(ApiRoutes.DeliveryPoint.AddSync, new DeliveryPointDto { type = type, value = value });
            var errorResponseDto = await responseDuplicated.Content.ReadFromJsonAsync<ErrorResponseDto>();

            response.EnsureSuccessStatusCode();
            Assert.False(responseDuplicated.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.DeliveryPointAlreadyExist, value), errorResponseDto?.Error);
        }
        #endregion

    }
}
