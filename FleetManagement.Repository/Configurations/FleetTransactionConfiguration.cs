using FleetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Repository.Configurations
{
    internal class FleetTransactionConfiguration : IEntityTypeConfiguration<FleetTransaction>
    {
        public void Configure(EntityTypeBuilder<FleetTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Barcode).IsRequired().HasMaxLength(11);

            builder.Property(x => x.State).IsRequired();

            builder.Property(x => x.TransactionId).IsRequired();

            builder.Property(x => x.Message).HasMaxLength(255);

            builder.ToTable("FleetTransactions");
        }
    }
}
