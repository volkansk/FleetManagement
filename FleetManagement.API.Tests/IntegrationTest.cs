using FleetManagement.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;

namespace FleetManagement.API.Tests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                        if (descriptor != null) services.Remove(descriptor);

                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });
                    });
                });

            TestClient = appFactory.CreateClient();

            SeedData(appFactory);
        }

        private static void SeedData(WebApplicationFactory<Program> appFactory)
        {
            var scopeFactory = appFactory.Server.Services.GetService<IServiceScopeFactory>();
            if (scopeFactory is null) return;

            using IServiceScope scope = scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<DataContext>();
            if (context is null) return;

            var vehicle = context.Vehicles.AddAsync(new Core.Entities.Vehicle { Plate = "34 TL 34" }).Result.Entity;

            var branch = context.DeliveryPoints.AddAsync(new Core.Entities.DeliveryPoint { Type = Core.Enumerations.DeliveryPointTypes.Branch, Value = 1 }).Result.Entity;
            var distributionCenter = context.DeliveryPoints.AddAsync(new Core.Entities.DeliveryPoint { Type = Core.Enumerations.DeliveryPointTypes.DistributionCenter, Value = 2 }).Result.Entity;
            var transferCenter = context.DeliveryPoints.AddAsync(new Core.Entities.DeliveryPoint { Type = Core.Enumerations.DeliveryPointTypes.TransferCenter, Value = 3 }).Result.Entity;

            var bag1 = context.Bags.AddAsync(new Core.Entities.Bag { Barcode = "C725799", DeliveryPointId = distributionCenter.Id }).Result.Entity;
            var bag2 = context.Bags.AddAsync(new Core.Entities.Bag { Barcode = "C725800", DeliveryPointId = transferCenter.Id }).Result.Entity;

            var packageBranch1 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P7988000121", DeliveryPointId = branch.Id, VolumetricWeight = 5 }).Result.Entity;
            var packageBranch2 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P7988000122", DeliveryPointId = branch.Id, VolumetricWeight = 5 }).Result.Entity;
            var packageBranch3 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P7988000123", DeliveryPointId = branch.Id, VolumetricWeight = 9 }).Result.Entity;

            var packageDistributionCenter1 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000120", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 33 }).Result.Entity;
            var packageDistributionCenter2 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000121", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 17 }).Result.Entity;
            var packageDistributionCenter3 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000122", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 26, BagId = bag1.Id }).Result.Entity;
            var packageDistributionCenter4 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000123", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 35 }).Result.Entity;
            var packageDistributionCenter5 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000124", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 1 }).Result.Entity;
            var packageDistributionCenter6 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000125", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 200 }).Result.Entity;
            var packageDistributionCenter7 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P8988000126", DeliveryPointId = distributionCenter.Id, VolumetricWeight = 50, BagId = bag1.Id }).Result.Entity;

            var packageTransferCenter1 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P9988000126", DeliveryPointId = transferCenter.Id, VolumetricWeight = 15 }).Result.Entity;
            var packageTransferCenter2 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P9988000127", DeliveryPointId = transferCenter.Id, VolumetricWeight = 16 }).Result.Entity;
            var packageTransferCenter3 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P9988000128", DeliveryPointId = transferCenter.Id, VolumetricWeight = 55, BagId = bag2.Id }).Result.Entity;
            var packageTransferCenter4 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P9988000129", DeliveryPointId = transferCenter.Id, VolumetricWeight = 28, BagId = bag2.Id }).Result.Entity;
            var packageTransferCenter5 = context.Packages.AddAsync(new Core.Entities.Package { Barcode = "P9988000130", DeliveryPointId = transferCenter.Id, VolumetricWeight = 17 }).Result.Entity;

            context.SaveChanges();
        }

    }
}
