using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestBackend.Core.Models.Business;

namespace RestBackend.Data.Configurations
{
    public class _PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder
                 .HasKey(m => m.IdProperty);

            builder
                .Property(m => m.IdProperty)
                .UseIdentityColumn();

            builder
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.CodeInternal)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(p => p.Year)
                .IsRequired();

            builder
                .Property(p => p.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder
                .HasIndex(x => x.CodeInternal)
                .IsUnique();

            builder
                 .HasOne(m => m.Owner)
                 .WithMany(a => a.Properties)
                 .HasForeignKey(m => m.IdOwner);

        }
    }
}
