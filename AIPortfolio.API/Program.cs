using System.Text;
using AIPortfolio.API.Extensions;
using AIPortfolio.API.Middleware;
using AIPortfolio.Application;
using AIPortfolio.Infrastructure;
using AIPortfolio.Infrastructure.Configurations;
using AIPortfolio.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

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
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 
        StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (
        context,
        cancellationToken) =>
    {
        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message = "Too Many Requests. Try Again in 5 minutes"
            },
            cancellationToken);
    };
    options.AddFixedWindowLimiter(
        "AuthPolicy",
        configure =>
        {
            configure.PermitLimit = 5;
            configure.Window = TimeSpan.FromMinutes(5);
            configure.QueueLimit = 0;
        });
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}
app.UseCors("Frontend");

app.UseGlobalExceptionHandling();
app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();