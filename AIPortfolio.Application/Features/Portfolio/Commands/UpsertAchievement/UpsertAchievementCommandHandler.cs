using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.Helpers;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertAchievement;

internal sealed class UpsertAchievementCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertAchievementCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertAchievementCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var entity = new ResumeAchievement
        {
            Id = command.Id ?? 0,
            Title = command.Title,
            Description = command.Description,
            AchievedDate = DateParser.ParseNullableDate(command.AchievedDate),
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertAchievementAsync(entity);
        return id;
    }
}