using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCustomSection;

internal sealed class UpsertCustomSectionCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertCustomSectionCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertCustomSectionCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var entity = new ResumeCustomSection
        {
            Id = command.Id ?? 0,
            SectionTitle = command.SectionTitle,
            Content = command.Content,
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertCustomSectionAsync(entity);
        return id;
    }
}