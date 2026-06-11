using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Auth;
using FluentValidation;
using FluentValidation.Results;

namespace AIPortfolio.API.Endpoints.Auth;

internal static class LoginEndpoint
{
    public static async Task<IResult> PostLogin(
        LoginRequest loginRequest,
        IAuthService authSerivce,
        IValidator <LoginRequest> loginValidator)

    {
        ValidationResult validationResult =
            await loginValidator.ValidateAsync(loginRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult.ToDictionary());
        }
        LoginResponse? response =
            await authSerivce.LoginAsync(loginRequest);

        if (response is null)
        {
            return TypedResults.Unauthorized();
        }
        
        return TypedResults.Ok(response);
    }
    public static IResult GetMe(
        HttpContext httpContext)
    {
        IUserInfoAccessor userInfoAccessor =
            httpContext.RequestServices
                .GetRequiredService<IUserInfoAccessor>();

        return TypedResults.Ok(new
        {
            userInfoAccessor.UserId,
            userInfoAccessor.Email,
            userInfoAccessor.FullName,
            userInfoAccessor.IsAuthenticated
        });
    }
}