using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCustomSection;

internal sealed class UpsertCustomSectionCommandValidator : AbstractValidator<UpsertCustomSectionCommand>
{
    public UpsertCustomSectionCommandValidator()
    {
        RuleFor(x => x.SectionTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(4000);
    }
}