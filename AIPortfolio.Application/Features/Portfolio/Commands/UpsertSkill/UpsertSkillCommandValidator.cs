using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSkill;

internal sealed class UpsertSkillCommandValidator : AbstractValidator<UpsertSkillCommand>
{
    public UpsertSkillCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}