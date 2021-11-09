using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestBackend.Core.Models.Business;

namespace RestBackend.Data.Configurations
{
    public class _TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder
                 .HasKey(m => m.IdTax);

            builder
                .Property(m => m.IdTax)
                .UseIdentityColumn();

            builder
              .Property(m => m.Enabled)
              .IsRequired();

            builder
              .Property(m => m.Value)
              .HasColumnType("decimal(18,4)")
              .IsRequired();

            builder
                .ToTable("Tax");

        }
    }
}
