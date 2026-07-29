using AIPortfolio.Application.DTOs.Auth;
using FluentValidation;

namespace AIPortfolio.Application.Validators.Auth;

public sealed class RegisterRequestvalidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestvalidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Name can not be empty.")
            .MaximumLength(256)
            .WithMessage("Name cannot exceed 256 characters.")
            .MinimumLength(3)
            .WithMessage("Name cannot exceed 3 characters.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email can not be empty.")
            .EmailAddress()
            .WithMessage("Email can not be empty.");
    }
}