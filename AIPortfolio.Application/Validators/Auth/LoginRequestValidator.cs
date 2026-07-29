using AIPortfolio.Application.DTOs.Auth;
using FluentValidation;

namespace AIPortfolio.Application.Validators.Auth;

public sealed class 
    
    LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address");
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required");

    }
}