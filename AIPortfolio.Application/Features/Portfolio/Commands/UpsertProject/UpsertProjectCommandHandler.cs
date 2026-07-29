using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertProject;

internal sealed class UpsertProjectCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertProjectCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertProjectCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var entity = new ResumeProject
        {
            Id = command.Id ?? 0,
            Title = command.Title,
            Description = command.Description,
            TechStack = command.TechStack,
            ProjectUrl = command.ProjectUrl,
            GithubUrl = command.GithubUrl,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Role = command.Role,
            OrderIndex = command.OrderIndex,
            ThumbnailUrl = command.ThumbnailUrl,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertProjectAsync(entity);
        return id;
    }
}