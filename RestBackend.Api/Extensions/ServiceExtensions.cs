using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RestBackend.Core;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using RestBackend.Data;
using RestBackend.Infrastructure.Cache;
using RestBackend.Infrastructure.FileStore;
using RestBackend.Infrastructure.Notification;
using RestBackend.Services;

namespace RestBackend.Api.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Add Bussines and logic services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITaxService, TaxService>();
            services.AddTransient<IOwnerService, OwnerService>();
            services.AddTransient<IPropertyService, PropertyService>();

            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileStoreService, FileStoreService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            return services;
        }
    }
}
