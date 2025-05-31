using Billi.Backend.CrossCutting.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Billi.Backend.Infra.IoC.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception caught by middleware");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.AddApiVersionToContentType("application/json");

                var response = new Response(false, "An unexpected error occurred. Please try again later.");

                if (env.IsDevelopment())
                    response.Data = ex.InnerException;

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
