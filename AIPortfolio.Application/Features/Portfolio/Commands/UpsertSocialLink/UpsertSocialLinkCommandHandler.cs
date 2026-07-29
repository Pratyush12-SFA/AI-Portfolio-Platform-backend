using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Portfolio;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSocialLink;

internal sealed class UpsertSocialLinkCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertSocialLinkCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertSocialLinkCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var entity = new PortfolioSocialLink
        {
            Id = command.Id ?? 0,
            Platform = command.Platform,
            Url = command.Url,
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertSocialLinkAsync(entity);
        return id;
    }
}