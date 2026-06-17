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
        ICookieService cookieService,
        HttpContext httpContext,
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
            return Results.BadRequest("User already exists");
        }
        cookieService.SetRefreshTokenCookie(
            httpContext.Response,
            response.RefreshToken,
            true);

        return Results.Ok(
            new
            {
                response.AccessToken,
                response.Email,
                response.FullName,
            });
    }
}