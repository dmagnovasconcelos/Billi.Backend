using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.Infra.IoC.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Billi.Backend.Infra.IoC
{
    public static class ConfigureRequestPipeline
    {
        public static WebApplication Configure(this WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<TokenBlacklistMiddleware>();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllers();
            });

            app.UseSwaggerSetup();

            return app;
        }
    }
}
