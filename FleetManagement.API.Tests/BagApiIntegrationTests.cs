using FleetManagement.API.Tests.Utilities;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Service.Constants;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.API.Tests
{
    public class BagApiIntegrationTests : IntegrationTest
    {
        #region Return Success

        [Theory]
        [InlineData("C725799100", 2, "C725799100")]
        [InlineData("C725800100", 3, "C725800100")]
        public async Task AddBag_ShouldBeAdded_WhenGivenValidBag_ReturnSuccess(string barcode, int deliveryPointValue, string expected)
        {
            var responseDP = await TestClient.GetAsync(ApiRoutes.DeliveryPoint.GetByValueSync.Replace("{value}", deliveryPointValue.ToString()));
            DeliveryPointResultDto? deliveryPointDto = await responseDP.Content.ReadFromJsonAsync<DeliveryPointResultDto>();

            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var bagResultDto = await response.Content.ReadFromJsonAsync<BagResultDto>();

            response.EnsureSuccessStatusCode();
            Assert.NotNull(bagResultDto);
            Assert.Equal(expected, bagResultDto?.barcode);
            Assert.Equal(deliveryPointDto?.id, bagResultDto?.deliveryPointId);
        }

        #endregion

        #region Return Exception

        [Theory]
        [InlineData(null, 1, "barcode is required.")]
        [InlineData("", 3, "barcode is required.")]
        public async Task AddBag_ShouldNotBeAdded_WhenGivenNullOrEmptyBarcode_ReturnRequiredException(string barcode, int deliveryPointValue, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("VSI3489112003", 2, "Maximum length of barcode is 11")]
        public async Task AddBag_ShouldNotBeAdded_WhenGiven12LengthBarcode_ReturnMaximumLengthException(string barcode, int deliveryPointValue, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("C725803100", 0, "delivery Point Value must be greater 0.")]
        public async Task AddBag_ShouldNotBeAdded_WhenGivenNegativeDeliveryPointValue_ReturnGreaterException(string barcode, int deliveryPointValue, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("C725801100", 15)]
        public async Task AddBag_ShouldNotBeAdded_WhenGivenDuplicateBarcode_ReturnAlreadyExistException(string barcode, int deliveryPointValue)
        {
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.DeliveryPoint.AddSync, new DeliveryPointDto { type = Core.Enumerations.DeliveryPointTypes.TransferCenter, value = deliveryPointValue });

            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var responseDuplicated = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var errorResponseDto = await responseDuplicated.Content.ReadFromJsonAsync<ErrorResponseDto>();

            response.EnsureSuccessStatusCode();
            Assert.False(responseDuplicated.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.BagAlreadyExist, barcode), errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("C725802100", 2000)]
        public async Task AddBag_ShouldNotBeAdded_WhenGivenNotDefinedDeliveryPoint_ReturnNotFoundException(string barcode, int deliveryPointValue)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = barcode, deliveryPointValue = deliveryPointValue });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.DeliveryPointNotFound, deliveryPointValue), errorResponseDto?.Error);
        }
        #endregion

    }
}
