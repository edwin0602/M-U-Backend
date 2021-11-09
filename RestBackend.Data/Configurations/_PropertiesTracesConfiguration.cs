using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestBackend.Core.Models.Business;

namespace RestBackend.Data.Configurations
{
    public class _PropertiesTracesConfiguration : IEntityTypeConfiguration<PropertyTrace>
    {
        public void Configure(EntityTypeBuilder<PropertyTrace> builder)
        {
            builder
                 .HasKey(m => m.IdPropertyTrace);

            builder
                .Property(m => m.IdPropertyTrace)
                .UseIdentityColumn();

            builder
                .Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(m => m.DateSale)
                .IsRequired();

            builder
                .Property(m => m.Value)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder
                .Property(m => m.Tax)
                .HasColumnType("decimal(18,4)");

            builder
                 .HasOne(m => m.Property)
                 .WithMany(a => a.PropertiesTraces)
                 .HasForeignKey(m => m.IdProperty);

            builder
               .ToTable("PropertyTrace");

        }
    }
}
