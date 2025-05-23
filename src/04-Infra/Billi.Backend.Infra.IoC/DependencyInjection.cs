using Billi.Backend.CrossCutting.Configurations;
using Billi.Backend.Domain.ValueObjects;
using Billi.Backend.Infra.Data.Contexts;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Billi.Backend.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration, Assembly[] assemblies)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(assemblies);

            services.AddAuthenticationConfig(configuration);

            services.AddScoped<AccessToken>();

            services.AddDatabase(configuration);

            foreach (var assembly in assemblies)
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
                services.AddValidatorsFromAssembly(assembly);
                services.AddAutoMapper(assembly);
            }

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.Where(t => t.Name.EndsWith("UnitOfWork")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SqlDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if DEBUG
                    .LogTo(Console.WriteLine)
#endif
            );

            return services;
        }

        private static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var securityKey = configuration["AuthenticationConfig:SecurityKey"];
            ArgumentNullException.ThrowIfNull(securityKey);

            services.Configure<AuthenticationOptions>(configuration.GetSection("AuthenticationConfig"));

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AuthenticationConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["AuthenticationConfig:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
            });
        }
    }
}
