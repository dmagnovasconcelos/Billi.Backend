using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.CrossCutting.Configurations;
using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.Infra.Data.Contexts;
using Billi.Backend.Infra.IoC.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
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

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                options.Conventions.Add(new RoutePrefixConvention("api/v{version:apiVersion}"));
            });

            services.AddEndpointsApiExplorer();

            services.AddHttpClient();
            services.AddAuthenticationConfig(configuration);
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddDatabase(configuration);
            services.AddRedis(configuration);

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

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration["Redis:ConnectionString"];
            ArgumentNullException.ThrowIfNull(redisConnection);

            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnection));

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(delegate (ApiVersioningOptions p)
            {
                p.DefaultApiVersion = new ApiVersion(2, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            }).AddApiExplorer(delegate (ApiExplorerOptions p)
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });  

            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.EnableAnnotations();

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IApiVersionDescriptionProvider requiredService = ServiceProviderServiceExtensions.GetRequiredService<IApiVersionDescriptionProvider>(provider);
                    foreach (ApiVersionDescription apiVersionDescription in requiredService.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(apiVersionDescription.GroupName, new OpenApiInfo
                        {
                            Version = "v" + apiVersionDescription.ApiVersion.ToString(),
                        });
                    }
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Basic scheme."
                });

                options.OperationFilter<BasicAuthOperationFilter>();

                // Adiciona XML Comments
                var assembly = Assembly.GetCallingAssembly();
                var xmlFiles = assembly
                    .GetReferencedAssemblies()
                    .Append(assembly.GetName())
                    .Select(a => Path.Combine(Path.GetDirectoryName(assembly.Location)!, a.Name + ".xml"))
                    .Where(File.Exists);

                foreach (var file in xmlFiles)
                    options.IncludeXmlComments(file);
            });

            return services;
        }
        
        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var securityKey = configuration["AuthenticationConfig:SecurityKey"];
            ArgumentNullException.ThrowIfNull(securityKey);

            services.Configure<AuthenticationSettings>(configuration.GetSection("AuthenticationConfig"));

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = true,
                        ValidIssuer = configuration["AuthenticationConfig:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["AuthenticationConfig:Audience"],
                        ValidateLifetime = true,
                    };
                });


            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BaseDbContext, SqlDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if DEBUG
                    .LogTo(Console.WriteLine)
#endif
            );

            return services;
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class BasicAuthAttribute : Attribute
        {
        }

        public class BasicAuthOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var hasBasicAuthAttribute = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<BasicAuthAttribute>()
                    .Any();

                operation.Security ??= [];

                if (hasBasicAuthAttribute)
                {                    
                    var openApiSecurityScheme = new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Basic"
                        }
                    };

                    operation.Security.Add(new OpenApiSecurityRequirement { [openApiSecurityScheme] = [] });
                }
                else
                {
                    
                    var openApiSecurityScheme = new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    };

                    operation.Security.Add(new OpenApiSecurityRequirement { [openApiSecurityScheme] = [] });
                }                
            }
        }
    }
}