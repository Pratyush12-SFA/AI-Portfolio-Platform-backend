using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertEducation;

internal sealed class UpsertEducationCommandValidator : AbstractValidator<UpsertEducationCommand>
{
    public UpsertEducationCommandValidator()
    {
        RuleFor(x => x.Institution).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Degree).NotEmpty().MaximumLength(200);
    }
}