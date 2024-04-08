using FleetManagement.Core.Repositories;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Core.Entities;
using FleetManagement.Service.Services;
using Moq;
using Xunit;

namespace FleetManagement.Service.Tests
{
    public class PackageTests
    {
        private PackageService packageService { get; set; }

        private readonly Mock<IPackageRepository> packageRepository = new();
        private readonly Mock<IBagRepository> bagRepository = new();
        private readonly Mock<IDeliveryPointRepository> deliveryPointRepository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();

        public PackageTests()
        {
            this.packageService = new PackageService(packageRepository.Object, bagRepository.Object, deliveryPointRepository.Object, unitOfWork.Object);
        }

        [Theory]
        [InlineData("P123456", "P123456", 1, 1, 15)]
        public async void AddPackage_ShouldBeCreated_WhenGivenValue_ReturnSuccess(string barcode, string expectedBarcode, int deliveryPointValue, int expectedDeliveryPointValue, int volumetricWeight)
        {
            deliveryPointRepository.Setup(repo => repo.GetSingleDeliveryPointByValueAsync(It.IsAny<int>())).ReturnsAsync(new DeliveryPoint { Id = deliveryPointValue });

            Package package = await packageService.AddAsync(new Package
            {
                Barcode = barcode,
                DeliveryPoint = new DeliveryPoint { Value = deliveryPointValue },
                VolumetricWeight = volumetricWeight
            });

            Assert.Equal(expectedBarcode, package.Barcode);
            Assert.Equal(expectedDeliveryPointValue, package.DeliveryPoint?.Value);
        }

        [Theory]
        [InlineData("P123456", "C123456", 2)]
        public async void AssignPackageToBag_ShouldBeAssigned_WhenGivenValue_ReturnSuccess(string packageBarcode, string bagBarcode, int deliveryPointId)
        {
            bagRepository.Setup(repo => repo.GetSingleBagByBarcodeAsync(It.IsAny<string>())).ReturnsAsync(new Bag { Barcode = bagBarcode, DeliveryPointId = deliveryPointId });
            packageRepository.Setup(repo => repo.GetSinglePackageByBarcodeAsync(It.IsAny<string>())).ReturnsAsync(new Package { Barcode = bagBarcode, DeliveryPointId = deliveryPointId });

            try
            {
                await packageService.AssignPackageToBag(new Core.DTOs.Input.AssignSinglePackageDto
                {
                    PackageBarcode = packageBarcode,
                    BagBarcode = bagBarcode
                });
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }
}