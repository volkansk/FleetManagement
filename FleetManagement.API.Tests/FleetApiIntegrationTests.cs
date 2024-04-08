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
    public class FleetApiIntegrationTests : IntegrationTest
    {
        #region Return Success

        [Theory, Trait("Priority", "1"), Trait("Category", "CaseResult")]
        [InlineData("34 TL 34", 4, 4, 4, 3, 3, 4, 4, 4, 4, 3, 3, 4, 4, 3)]
        public async Task Distribute_ShouldBeDistribute_WhenGivenValidCommand_ReturnExpectedResult(string expectedPlate, int P7988000121State, int P7988000122State, int P7988000123State, int P8988000121State, int C725799State, int P8988000123State, int P8988000124State, int P8988000125State, int C725799State2, int P9988000126State, int P9988000127State, int P9988000128State, int P9988000129State, int P9988000130State)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Fleet.DistributeAsync, DefaultDistributionCommandDto);

            var distributionResultDto = await response.Content.ReadFromJsonAsync<DistributionResultDto>();

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.NotNull(distributionResultDto);

            if (distributionResultDto is not null)
            {
                Assert.Equal(expectedPlate, distributionResultDto.plate);
                Assert.Equal((int)DeliveryPointTypes.Branch, distributionResultDto.route[0].deliveryPoint);
                Assert.Equal(P7988000121State, distributionResultDto.route[0].deliveries[0].state);
                Assert.Equal(P7988000122State, distributionResultDto.route[0].deliveries[1].state);
                Assert.Equal(P7988000123State, distributionResultDto.route[0].deliveries[2].state);
                Assert.Equal(P8988000121State, distributionResultDto.route[0].deliveries[3].state);
                Assert.Equal(C725799State, distributionResultDto.route[0].deliveries[4].state);

                Assert.Equal((int)DeliveryPointTypes.DistributionCenter, distributionResultDto.route[1].deliveryPoint);
                Assert.Equal(P8988000123State, distributionResultDto.route[1].deliveries[0].state);
                Assert.Equal(P8988000124State, distributionResultDto.route[1].deliveries[1].state);
                Assert.Equal(P8988000125State, distributionResultDto.route[1].deliveries[2].state);
                Assert.Equal(C725799State2, distributionResultDto.route[1].deliveries[3].state);

                Assert.Equal((int)DeliveryPointTypes.TransferCenter, distributionResultDto.route[2].deliveryPoint);
                Assert.Equal(P9988000126State, distributionResultDto.route[2].deliveries[0].state);
                Assert.Equal(P9988000127State, distributionResultDto.route[2].deliveries[1].state);
                Assert.Equal(P9988000128State, distributionResultDto.route[2].deliveries[2].state);
                Assert.Equal(P9988000129State, distributionResultDto.route[2].deliveries[3].state);
                Assert.Equal(P9988000130State, distributionResultDto.route[2].deliveries[4].state);
            }
        }

        [Theory, Trait("Category", "PartialDistribute")]
        [InlineData(90, 0)]
        public async Task Distribute_ShouldBeDistribute_WhenGivenNotDefinedDeliveryPoint_ReturnPartialSuccess(int deliveryPointValue, int expectedState)
        {
            var commandDto = DefaultDistributionCommandDto;
            commandDto.route[0].deliveryPoint = deliveryPointValue;

            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Fleet.DistributeAsync, commandDto);
            var distributionResultDto = await response.Content.ReadFromJsonAsync<DistributionResultDto>();

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.NotNull(distributionResultDto);

            if (distributionResultDto is not null)
            {
                Assert.Equal(deliveryPointValue, distributionResultDto.route[0].deliveryPoint);
                Assert.Equal(expectedState, distributionResultDto.route[0].deliveries[0].state);
            }
        }

        #endregion

        #region Return Exception

        [Theory]
        [InlineData("3412 YT 195")]
        public async Task Distribute_ShouldNotBeDistribute_WhenGivenNotDefinedVehicle_ReturnNotFoundException(string plate)
        {
            var commandDto = DefaultDistributionCommandDto;
            commandDto.plate = plate;

            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Fleet.DistributeAsync, commandDto);
            var errorResponseDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            Assert.False(response.IsSuccessStatusCode);
            Assert.NotNull(errorResponseDto);
            Assert.Contains(string.Format(Messages.VehicleNotFound, plate), errorResponseDto?.Error);
        }

        #endregion

        #region Private Methods
        private static DistributionCommandDto DefaultDistributionCommandDto => new()
        {
            plate = "34 TL 34",
            route = new System.Collections.Generic.List<DistributionCommandDto.Route>() {
                    new DistributionCommandDto.Route()
                    {
                        deliveryPoint = 1,
                        deliveries = new System.Collections.Generic.List<DistributionCommandDto.Route.Delivery>()
                        {
                            new DistributionCommandDto.Route.Delivery(){barcode = "P7988000121"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P7988000122"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P7988000123"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P8988000121"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "C725799"}
                        }
                    },
                    new DistributionCommandDto.Route()
                    {
                        deliveryPoint = 2,
                        deliveries = new System.Collections.Generic.List<DistributionCommandDto.Route.Delivery>()
                        {
                            new DistributionCommandDto.Route.Delivery(){barcode = "P8988000123"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P8988000124"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P8988000125"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "C725799"}
                        }
                    },
                    new DistributionCommandDto.Route()
                    {
                        deliveryPoint = 3,
                        deliveries = new System.Collections.Generic.List<DistributionCommandDto.Route.Delivery>()
                        {
                            new DistributionCommandDto.Route.Delivery(){barcode = "P9988000126"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P9988000127"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P9988000128"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P9988000129"},
                            new DistributionCommandDto.Route.Delivery(){barcode = "P9988000130"}
                        }
                    }
                }
        };

        #endregion
    }
}
