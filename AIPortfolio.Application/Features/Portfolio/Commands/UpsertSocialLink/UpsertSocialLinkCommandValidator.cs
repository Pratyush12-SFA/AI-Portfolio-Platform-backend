using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSocialLink;

internal sealed class UpsertSocialLinkCommandValidator : AbstractValidator<UpsertSocialLinkCommand>
{
    public UpsertSocialLinkCommandValidator()
    {
        RuleFor(x => x.Platform).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(2000);
    }
}