using FleetManagement.API.Tests.Utilities;
using FleetManagement.Core.DTOs.Input;
using FleetManagement.Core.DTOs.Output;
using FleetManagement.Service.Constants;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FleetManagement.API.Tests
{
    public class PackageApiIntegrationTests : IntegrationTest
    {
        #region Return Success

        #region AddAsync
        [Theory]
        [InlineData("P7988121", 1, 5, "P7988121")]
        [InlineData("P7988122", 1, 5, "P7988122")]
        [InlineData("P7988123", 1, 9, "P7988123")]
        [InlineData("P8988120", 2, 33, "P8988120")]
        [InlineData("P8988121", 2, 17, "P8988121")]
        [InlineData("P8988122", 2, 26, "P8988122")]
        [InlineData("P8988123", 2, 35, "P8988123")]
        [InlineData("P8988124", 2, 1, "P8988124")]
        [InlineData("P8988125", 2, 200, "P8988125")]
        [InlineData("P8988126", 2, 50, "P8988126")]
        [InlineData("P9988126", 3, 15, "P9988126")]
        [InlineData("P9988127", 3, 16, "P9988127")]
        [InlineData("P9988128", 3, 55, "P9988128")]
        [InlineData("P9988129", 3, 28, "P9988129")]
        [InlineData("P9988130", 3, 17, "P9988130")]
        public async Task AddPackage_ShouldBeAdded_WhenGivenValidPackage_ReturnSuccess(string barcode, int deliveryPointValue, int volumetricWeight, string expected)
        {
            var responseDP = await TestClient.GetAsync(ApiRoutes.DeliveryPoint.GetByValueSync.Replace("{value}", deliveryPointValue.ToString()));
            DeliveryPointResultDto? deliveryPointDto = await responseDP.Content.ReadFromJsonAsync<DeliveryPointResultDto>();

            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var packageResultDto = await response.Content.ReadFromJsonAsync<PackageResultDto>();

            response.EnsureSuccessStatusCode();
            Assert.NotNull(packageResultDto);
            Assert.Equal(expected, packageResultDto?.barcode);
            Assert.Equal(deliveryPointDto?.id, packageResultDto?.deliveryPointId);
        }
        #endregion

        #region AssignSinglePackage
        [Theory]
        [InlineData("P1988000122", "C125799", 2, "OK")]
        [InlineData("P1988000126", "C125799", 2, "OK")]
        [InlineData("P1988000128", "C125800", 3, "OK")]
        [InlineData("P1988000129", "C125800", 3, "OK")]
        public async Task AssignSinglePackage_ShouldBeAssigned_WhenGivenValidBarcodes_ReturnSuccess(string packageBarcode, string bagBarcode, int deliveryPointValue, string expected)
        {
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = packageBarcode, deliveryPointValue = deliveryPointValue, volumetricWeight = 12 });

            var responseBag = await TestClient.GetAsync(ApiRoutes.Bag.GetByBarcodeSync.Replace("{barcode}", bagBarcode));
            if (!responseBag.IsSuccessStatusCode)
                _ = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = bagBarcode, deliveryPointValue = deliveryPointValue });

            _ = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = bagBarcode, deliveryPointValue = deliveryPointValue });

            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var assignSinglePackage = await response.Content.ReadFromJsonAsync<string>();

            response.EnsureSuccessStatusCode();
            Assert.NotNull(assignSinglePackage);
            Assert.Equal(expected, assignSinglePackage);
        }
        #endregion

        #endregion

        #region Return Exception

        #region AddAsync
        [Theory]
        [InlineData(null, 107, 1, "barcode is required.")]
        [InlineData("", 3, 2, "barcode is required.")]
        public async Task AddPackage_ShouldNotBeAdded_WhenGivenNullOrEmptyBarcode_ReturnRequiredException(string barcode, int deliveryPointValue, int volumetricWeight, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P000800013212", 108, 45, "Maximum length of barcode is 11")]
        public async Task AddPackage_ShouldNotBeAdded_WhenGiven12LengthBarcode_ReturnMaximumLengthException(string barcode, int deliveryPointValue, int volumetricWeight, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P0008000133", 0, 45, "delivery Point Value must be greater 0.")]
        public async Task AddPackage_ShouldNotBeAdded_WhenGivenNegativeDeliveryPointValue_ReturnGreaterException(string barcode, int deliveryPointValue, int volumetricWeight, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P0008000134", 1, 0, "volumetric Weight must be greater 0.")]
        public async Task AddPackage_ShouldNotBeAdded_WhenGivenNegativeVolumetricWeight_ReturnGreaterException(string barcode, int deliveryPointValue, int volumetricWeight, string expected)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P0008000130", 105, 100)]
        public async Task AddPackage_ShouldNotBeAdded_WhenGivenDuplicateBarcode_ReturnAlreadyExistException(string barcode, int deliveryPointValue, int volumetricWeight)
        {
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.DeliveryPoint.AddSync, new DeliveryPointDto { type = Core.Enumerations.DeliveryPointTypes.TransferCenter, value = deliveryPointValue });

            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var responseDuplicated = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var errorResponseDto = await responseDuplicated.Content.ReadFromJsonAsync<ErrorResponseDto>();

            response.EnsureSuccessStatusCode();
            Assert.False(responseDuplicated.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.PackageAlreadyExist, barcode), errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P0008000131", 1060, 100)]
        public async Task AddPackage_ShouldNotBeAdded_WhenGivenNotDefinedDeliveryPoint_ReturnNotFoundException(string barcode, int deliveryPointValue, int volumetricWeight)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = barcode, deliveryPointValue = deliveryPointValue, volumetricWeight = volumetricWeight });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.DeliveryPointNotFound, deliveryPointValue), errorResponseDto?.Error);
        }
        #endregion

        #region AssignPackageToBag
        [Theory]
        [InlineData(null, "C7257099", "Package Barcode is required.")]
        [InlineData("", "C7257099", "Package Barcode is required.")]
        public async Task AssignSinglePackage_ShouldBeAssigned_WhenGivenNullOrEmptyPackageBarcode_ReturnRequiredException(string packageBarcode, string bagBarcode, string expected)
        {
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P000800013212", "C7205799", "Maximum length of Package Barcode is 11")]
        public async Task AssignSinglePackage_ShouldBeAssigned_WhenGiven12LengthPackageBarcode_ReturnMaximumLengthException(string packageBarcode, string bagBarcode, string expected)
        {
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P0008000131", null, "Bag Barcode is required.")]
        [InlineData("P0008000131", "", "Bag Barcode is required.")]
        public async Task AssignSinglePackage_ShouldBeAssigned_WhenGivenNullOrEmptyBagBarcode_ReturnRequiredException(string packageBarcode, string bagBarcode, string expected)
        {
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P0008000131", "C72579989012", "Maximum length of Bag Barcode is 11")]
        public async Task AssignSinglePackage_ShouldBeAssigned_WhenGiven12LengthBagBarcode_ReturnMaximumLengthException(string packageBarcode, string bagBarcode, string expected)
        {
            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(expected, errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P808000122", "C72KS1")]
        public async Task AssignSinglePackage_ShouldBeAssigned_WhenGivenNotDefinedBagBarcode_ReturnNotFoundException(string packageBarcode, string bagBarcode)
        {
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = packageBarcode, deliveryPointValue = 1, volumetricWeight = 12 });

            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.BagNotFound, bagBarcode), errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P898KSDLsd1", "CY123456")]
        public async Task AssignSinglePackage_ShouldNotBeAssigned_WhenGivenNotDefinedPackageBarcode_ReturnNotFoundException(string packageBarcode, string bagBarcode)
        {
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = bagBarcode, deliveryPointValue = 1 });

            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.PackageNotFound, packageBarcode), errorResponseDto?.Error);
        }

        [Theory]
        [InlineData("P7988000000", "C72580000")]
        public async Task AssignSinglePackage_ShouldNotBeAssigned_WhenGivenDifferentDP_ReturnNotSameDPException(string packageBarcode, string bagBarcode)
        {
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.Package.AddSync, new PackageDto { barcode = packageBarcode, deliveryPointValue = 1, volumetricWeight = 12 });
            _ = await TestClient.PostAsJsonAsync(ApiRoutes.Bag.AddSync, new BagDto { barcode = bagBarcode, deliveryPointValue = 2 });

            var response = await TestClient.PutAsJsonAsync(ApiRoutes.Package.AssignSinglePackageAsync, new AssignSinglePackageDto { PackageBarcode = packageBarcode, BagBarcode = bagBarcode });
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(Messages.NotSamePackageBagDeliveryPoint, errorResponseDto?.Error);
        }
        #endregion

        #endregion
    }
}
