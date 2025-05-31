using Billi.Backend.Application.Commands.Auth;
using Billi.Backend.CrossCutting.Contexts;
using Billi.Backend.Domain.ValueObjects;
using Billi.Backend.Infra.Data.Contexts;
using Billi.Backend.Infra.IoC;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwagger();

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

builder.Services.AddApplicationDependencies(builder.Configuration,
    [
        Assembly.GetExecutingAssembly(),
        typeof(AuthCommand).Assembly,
        typeof(AccessToken).Assembly,
        typeof(SqlDbContext).Assembly,
        typeof(BaseDbContext).Assembly,
    ]
);

var app = builder.Configure();

// Configure the HTTP request pipeline.

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();


await app.RunAsync();