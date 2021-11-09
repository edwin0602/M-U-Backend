using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestBackend.Api.Wrappers;
using RestBackend.Core.Models.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RestBackend.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    await HandleUnauthorized(httpContext);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessException(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleUnauthorized(HttpContext context)
        {
            _logger.LogError($"Request Access Denied: {context.Request.Path}");

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new ErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Request Access Denied"
            }.ToString());
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            _logger.LogError($"Exception: {exception.Message}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

        private async Task HandleBusinessException(HttpContext context, Exception exception)
        {
            _logger.LogError($"Business Exception: {exception.Message}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}
