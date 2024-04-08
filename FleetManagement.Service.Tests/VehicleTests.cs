using FleetManagement.Core.Repositories;
using FleetManagement.Core.UnitOfWorks;
using FleetManagement.Core.Entities;
using FleetManagement.Service.Services;
using Moq;
using Xunit;

namespace FleetManagement.Service.Tests
{
    public class VehicleTests
    {
        private VehicleService vehicleService { get; set; }

        private readonly Mock<IVehicleRepository> vehicleRepository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();

        public VehicleTests()
        {
            this.vehicleService = new VehicleService(vehicleRepository.Object, unitOfWork.Object);
        }

        [Theory]
        [InlineData("34 TL 34", "34 TL 34")]
        [InlineData("34 TL 314", "34 TL 314")]
        public async void AddVehicle_ShouldBeCreated_WhenGivenValue_ReturnSuccess(string plate, string expectedPlate)
        {
            Vehicle vehicle = await vehicleService.AddAsync(new Vehicle
            {
                Plate = plate
            });

            Assert.Equal(expectedPlate, vehicle.Plate);
        }
    }
}