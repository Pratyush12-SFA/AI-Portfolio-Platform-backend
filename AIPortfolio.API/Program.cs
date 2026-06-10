using System.Text;
using AIPortfolio.API.Extensions;
using AIPortfolio.Application;
using AIPortfolio.Infrastructure;
using AIPortfolio.Infrastructure.Configurations;
using AIPortfolio.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddEndpointMappers();

JwtSettings jwtSettings =
    builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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
                            jwtSettings.SecretKey))
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();