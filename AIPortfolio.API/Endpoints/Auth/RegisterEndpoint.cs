using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using FluentValidation;
using FluentValidation.Results;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class RegisterEndpoint
{
    public static async Task<IResult> PostRegister(
        RegisterRequest registerRequest,
        IAuthService authService,
        IValidator<RegisterRequest> requestValidator)
    {
        ValidationResult validationResult = 
            await requestValidator.ValidateAsync(registerRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult.ToDictionary());
        }
        LoginResponse? response =
            await authService.RegisterAsync(registerRequest);

        if (response is null)
        {
            return Results.Conflict("User already exists");
        }

        return Results.Ok(response);
    }
}