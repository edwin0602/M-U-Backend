using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestBackend.Core;
using RestBackend.Core.Models.Auth;
using RestBackend.Core.Models.Business;
using RestBackend.Core.Models.Security;
using RestBackend.Core.Repositories;
using RestBackend.Data.Configurations;
using System;
using System.Threading.Tasks;

namespace RestBackend.Data
{
    public class RestBackendDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Audit> Audit { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        public DbSet<PropertyTrace> PropertyTraces { get; set; }

        public RestBackendDbContext() { }

        public RestBackendDbContext(DbContextOptions<RestBackendDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("edm");

            builder
                .ApplyConfiguration(new _OwnerConfiguration())
                .ApplyConfiguration(new _AuditConfiguration())
                .ApplyConfiguration(new _PropertiesImagesConfiguration())
                .ApplyConfiguration(new _PropertiesTracesConfiguration())
                .ApplyConfiguration(new _PropertyConfiguration())
                .ApplyConfiguration(new _TaxConfiguration());

            DataSeed(builder);
        }

        private void DataSeed(ModelBuilder builder)
        {
            #region [ Create Roles ]

            var GuidAdminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Administrador",
                NormalizedName = "Administrador".ToUpper()
            };
            var GuidAssistRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Asistente",
                NormalizedName = "Asistente".ToUpper()
            };

            builder.Entity<Role>().HasData(
               GuidAssistRole,
               GuidAdminRole
            );

            #endregion

            #region [ Add Admin user ]

            string EmailAdmin = "edwinman1991@gmail.com";
            string Admin = "admin";
            Guid GuidAdmin = Guid.NewGuid();

            var AdminUser = new User
            {
                Id = GuidAdmin,
                UserName = Admin,
                NormalizedUserName = Admin.ToUpper(),
                Email = EmailAdmin,
                NormalizedEmail = EmailAdmin.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            PasswordHasher<User> ph = new PasswordHasher<User>();
            AdminUser.PasswordHash = ph.HashPassword(AdminUser, "Tempora02");

            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = GuidAdminRole.Id,
                UserId = GuidAdmin
            });

            builder.Entity<User>().HasData(AdminUser);

            #endregion

            #region [ Add Default Tax ]

            builder.Entity<Tax>().HasData(new Tax
            {
                IdTax = 1,
                Enabled = true,
                Value = 0.19m
            });

            #endregion
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
