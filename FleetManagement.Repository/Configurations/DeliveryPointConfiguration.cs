using FleetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Repository.Configurations
{
    internal class DeliveryPointConfiguration : IEntityTypeConfiguration<DeliveryPoint>
    {
        public void Configure(EntityTypeBuilder<DeliveryPoint> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.Value).IsUnique();

            builder.Property(x => x.Type);

            builder.ToTable("DeliveryPoints");
        }
    }
}
