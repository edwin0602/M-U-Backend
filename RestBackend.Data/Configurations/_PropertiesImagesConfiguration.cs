using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestBackend.Core.Models.Business;

namespace RestBackend.Data.Configurations
{
    public class _PropertiesImagesConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder
                 .HasKey(m => m.IdProperyImage);

            builder
                .Property(m => m.IdProperyImage)
                .UseIdentityColumn();

            builder
                .Property(m => m.File)
                .IsRequired();

            builder
                .Property(m => m.Enabled)
                .IsRequired();

            builder
                 .HasOne(m => m.Property)
                 .WithMany(a => a.PropertiesImages)
                 .HasForeignKey(m => m.IdProperty);

            builder
                .ToTable("PropertyImage");

        }
    }
}
