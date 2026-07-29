using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteSocialLink;

internal sealed class DeleteSocialLinkCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteSocialLinkCommand, Result>
{
    public async Task<Result> Handle(DeleteSocialLinkCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteSocialLinkAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}