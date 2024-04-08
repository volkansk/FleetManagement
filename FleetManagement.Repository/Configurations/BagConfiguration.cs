using FleetManagement.Core.Entities;
using FleetManagement.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Repository.Configurations
{
    internal class BagConfiguration : IEntityTypeConfiguration<Bag>
    {
        public void Configure(EntityTypeBuilder<Bag> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.Barcode).IsUnique();
            builder.Property(x => x.Barcode).IsRequired().HasMaxLength(11);

            builder.Property(x => x.State).HasDefaultValue(BagStatuses.Created);

            builder.ToTable("Bags");
        }
    }
}
