using FleetManagement.Core.Entities;
using FleetManagement.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Repository.Configurations
{
    internal class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.Barcode).IsUnique();
            builder.Property(x => x.Barcode).IsRequired().HasMaxLength(11);

            builder.Property(x => x.State).HasDefaultValue(PackageStatuses.Created);

            builder.Property(x => x.VolumetricWeight).IsRequired();

            builder.Property(x => x.BagId);
            builder.Property(x => x.DeliveryPointId);

            builder.ToTable("Packages");

            //builder.HasOne(x => x.DeliveryPoint).WithMany(x => x.Packages).HasForeignKey(x => x.DeliveryPointId);
            //builder.HasOne(x => x.Bag).WithMany(x => x.Packages).HasForeignKey(x => x.BagId);
        }
    }
}
