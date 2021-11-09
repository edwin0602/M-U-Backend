using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestBackend.Api.Extensions;
using RestBackend.Data;

namespace RestBackend.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var dataAssemblyName = typeof(RestBackendDbContext).Assembly.GetName().Name;
            services.AddDbContext<RestBackendDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly(dataAssemblyName)));

            services.AddServices();

            services.AddSwagger();

            services.AddAutoMapper(typeof(RestBackend.Core.Mapping.MappingProfile));

            services.AddAuth(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.ConfigureSwagger();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureExceptionHandler();

            app.UseCors("AllowAnyOrigin");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}