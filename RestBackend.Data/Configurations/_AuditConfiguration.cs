using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestBackend.Core.Models.Security;

namespace RestBackend.Data.Configurations
{
    public class _AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder
                 .HasKey(m => m.IdAudit);

            builder
              .Property(m => m.Resource)
              .IsRequired();

            builder
              .Property(m => m.CreatedAt)
              .IsRequired();

            builder
                .ToTable("Audit");

        }
    }
}
