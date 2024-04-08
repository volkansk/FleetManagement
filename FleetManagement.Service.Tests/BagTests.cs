using FleetManagement.Core.Repositories;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Core.Entities;
using FleetManagement.Service.Services;
using Moq;
using Xunit;

namespace FleetManagement.Service.Tests
{
    public class BagTests
    {
        private BagService bagService { get; set; }

        private readonly Mock<IBagRepository> bagRepository = new();
        private readonly Mock<IDeliveryPointRepository> deliveryPointRepository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();

        public BagTests()
        {
            this.bagService = new BagService(unitOfWork.Object, bagRepository.Object, deliveryPointRepository.Object);
        }

        [Theory]
        [InlineData("C123456", "C123456", 1, 1)]
        public async void AddBag_ShouldBeCreated_WhenGivenValue_ReturnSuccess(string barcode, string expectedBarcode, int deliveryPointValue, int expectedDeliveryPointValue)
        {
            deliveryPointRepository.Setup(repo => repo.GetSingleDeliveryPointByValueAsync(It.IsAny<int>())).ReturnsAsync(new DeliveryPoint { Id = deliveryPointValue });

            Bag bag = await bagService.AddAsync(new Bag
            {
                Barcode = barcode,
                DeliveryPoint = new DeliveryPoint { Value = deliveryPointValue }
            });

            Assert.Equal(expectedBarcode, bag.Barcode);
            Assert.Equal(expectedDeliveryPointValue, bag.DeliveryPoint?.Value);
        }
    }
}