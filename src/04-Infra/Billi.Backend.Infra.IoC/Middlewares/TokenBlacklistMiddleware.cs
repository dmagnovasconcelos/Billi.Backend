using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.CrossCutting.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Infra.IoC.Middlewares
{
    public class TokenBlacklistMiddleware(RequestDelegate next, ILogger<TokenBlacklistMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context, ITokenBlacklistService tokenBlacklistService)
        {
            var authHeader = context.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith(CurrentUser.BearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader[CurrentUser.BearerPrefix.Length..].Trim();

                if (await tokenBlacklistService.IsTokenBlacklistedAsync(token))
                {
                    logger.LogWarning("Blacklisted token blocked: {Token}", token);

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new Response(false, "Token is blacklisted"));
                    return;
                }
            }

            await next(context);
        }
    }
}
