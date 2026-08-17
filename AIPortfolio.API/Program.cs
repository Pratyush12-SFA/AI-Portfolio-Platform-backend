using System.Text;
using System.Text.Json.Serialization;
using AIPortfolio.API.Extensions;
using AIPortfolio.API.Middleware;
using AIPortfolio.Application;
using AIPortfolio.Infrastructure;
using AIPortfolio.Infrastructure.Configurations;
using AIPortfolio.Persistence;
using AIPortfolio.Persistence.Data;
using AIPortfolio.Persistence.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddEndpointMappers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:5173");
        });
});

var jwtSettings =
    builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>();

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (jwtSettings?.SecretKey != null)
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,

                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                jwtSettings.SecretKey ?? throw new InvalidOperationException()))
                };
    });

  
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
using var scope = app.Services.CreateScope();
var db =
    scope.ServiceProvider.GetRequiredService<AIPortfolioDbContext>();
db.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await DevelopmentSeeder.SeedAsync(app.Services);
}

app.UseCors("Frontend");

app.UseGlobalExceptionHandling();


app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();