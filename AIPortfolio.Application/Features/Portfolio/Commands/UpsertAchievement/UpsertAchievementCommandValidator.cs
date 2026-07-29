using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertAchievement;

internal sealed class UpsertAchievementCommandValidator : AbstractValidator<UpsertAchievementCommand>
{
    public UpsertAchievementCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}