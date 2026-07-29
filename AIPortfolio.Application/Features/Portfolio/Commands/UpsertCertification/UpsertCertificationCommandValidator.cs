using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCertification;

internal sealed class UpsertCertificationCommandValidator : AbstractValidator<UpsertCertificationCommand>
{
    public UpsertCertificationCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}