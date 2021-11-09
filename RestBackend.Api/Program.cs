using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace RestBackend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseSerilog((HostBuilderContext context, LoggerConfiguration loggerConfiguration) =>
                {
                    loggerConfiguration
                        .Enrich.FromLogContext()
                        .ReadFrom
                            .Configuration(context.Configuration)
                        .WriteTo
                            .File(
                                new RenderedCompactJsonFormatter(),
                                "C:\\Logs\\log.json",
                                rollingInterval: RollingInterval.Day,
                                rollOnFileSizeLimit: true);
                });
    }
}
