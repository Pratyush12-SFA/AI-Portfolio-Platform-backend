using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertLanguage;

internal sealed class UpsertLanguageCommandValidator : AbstractValidator<UpsertLanguageCommand>
{
    public UpsertLanguageCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}