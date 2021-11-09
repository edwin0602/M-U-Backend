using Microsoft.AspNetCore.Builder;
using RestBackend.Api.Middlewares;

namespace RestBackend.Api.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
