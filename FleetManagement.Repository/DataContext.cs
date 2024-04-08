using FleetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FleetManagement.Repository
{
    public class DataContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DbSet<Bag> Bags { get; set; }
        public DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public DbSet<FleetTransaction> FleetTransactions { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
