using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestBackend.Core.Models.Business;

namespace RestBackend.Data.Configurations
{
    public class _OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder
                 .HasKey(m => m.IdOwner);

            builder
              .Property(m => m.Name)
              .HasMaxLength(200)
              .IsRequired();

            builder
              .Property(m => m.Address)
              .HasMaxLength(200);

            builder
              .Property(m => m.Birthday)
              .IsRequired();

            builder
                .Property(m => m.IdOwner)
                .UseIdentityColumn();

            builder
                .ToTable("Owner");

        }
    }
}
