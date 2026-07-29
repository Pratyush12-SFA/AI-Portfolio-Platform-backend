using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSkill;

internal sealed class UpsertSkillCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertSkillCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertSkillCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var entity = new ResumeSkill
        {
            Id = command.Id ?? 0,
            Name = command.Name,
            Category = command.Category,
            ProficiencyLevel = command.ProficiencyLevel,
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertSkillAsync(entity);
        return id;
    }
}