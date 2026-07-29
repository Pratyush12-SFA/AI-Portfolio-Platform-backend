using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertExperience;

internal sealed class UpsertExperienceCommandValidator : AbstractValidator<UpsertExperienceCommand>
{
    public UpsertExperienceCommandValidator()
    {
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.JobTitle).NotEmpty().MaximumLength(200);
    }
}