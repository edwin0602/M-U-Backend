using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using RestBackend.Security;
using RestBackend.Core.Models.Security;
using RestBackend.Core.Models.Auth;
using RestBackend.Data;
using Microsoft.AspNetCore.Identity;
using RestBackend.Core.Services.Infrastructure;

namespace RestBackend.Api.Extensions
{
    public static class AuthExtensions
    {
        /// <summary>
        /// Add Identity and JWT services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration Configuration)
        {
            #region [ Identity ]

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1d);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<RestBackendDbContext>()
            .AddDefaultTokenProviders();

            #endregion

            #region [ JWT ]

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddTransient<IJWTService, JWTService>();

            #endregion

            return services;
        }

        /// <summary>
        /// Add authentication services
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}