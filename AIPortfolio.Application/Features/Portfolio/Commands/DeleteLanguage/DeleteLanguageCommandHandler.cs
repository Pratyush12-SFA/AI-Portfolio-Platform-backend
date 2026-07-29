using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteLanguage;

internal sealed class DeleteLanguageCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteLanguageCommand, Result>
{
    public async Task<Result> Handle(DeleteLanguageCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteLanguageAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}