using FluentValidation;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertProject;

internal sealed class UpsertProjectCommandValidator : AbstractValidator<UpsertProjectCommand>
{
    public UpsertProjectCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}