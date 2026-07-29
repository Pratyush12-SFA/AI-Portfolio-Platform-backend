using AIPortfolio.Application.Abstractions;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteSkill;

internal sealed class DeleteSkillCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<DeleteSkillCommand, Result>
{
    public async Task<Result> Handle(DeleteSkillCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();
        await repository.DeleteSkillAsync(command.Id, userInfoAccessor.UserId);
        return Result.Success();
    }
}