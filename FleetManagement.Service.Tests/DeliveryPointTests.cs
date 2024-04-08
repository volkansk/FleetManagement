using FleetManagement.Core.Repositories;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Core.Entities;
using FleetManagement.Service.Services;
using Moq;
using Xunit;
using FleetManagement.Core.Enumerations;

namespace FleetManagement.Service.Tests
{
    public class DeliveryPointTests
    {
        private DeliveryPointService deliveryPointService { get; set; }

        private readonly Mock<IDeliveryPointRepository> deliveryPointRepository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();

        public DeliveryPointTests()
        {
            this.deliveryPointService = new DeliveryPointService(deliveryPointRepository.Object, unitOfWork.Object);
        }

        [Theory]
        [InlineData(DeliveryPointTypes.Branch, DeliveryPointTypes.Branch, 1, 1)]
        [InlineData(DeliveryPointTypes.DistributionCenter, DeliveryPointTypes.DistributionCenter, 2, 2)]
        [InlineData(DeliveryPointTypes.TransferCenter, DeliveryPointTypes.TransferCenter, 3, 3)]
        public async void AddDeliveryPoint_ShouldBeCreated_WhenGivenValue_ReturnSuccess(DeliveryPointTypes type, DeliveryPointTypes expectedType, int value, int expectedValue)
        {
            DeliveryPoint deliveryPoint = await deliveryPointService.AddAsync(new DeliveryPoint
            {
                Type = type,
                Value = value
            });

            Assert.Equal(expectedType, deliveryPoint.Type);
            Assert.Equal(expectedValue, deliveryPoint.Value);
        }
    }
}