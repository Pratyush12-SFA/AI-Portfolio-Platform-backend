using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertExperience;

internal sealed class UpsertExperienceCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertExperienceCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertExperienceCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var experience = new ResumeExperience
        {
            Id = command.Id ?? 0,
            CompanyName = command.CompanyName,
            JobTitle = command.JobTitle,
            Location = command.Location,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            IsCurrent = command.IsCurrent,
            Description = command.Description,
            Responsibilities = command.Responsibilities,
            EmploymentType = command.EmploymentType,
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertExperienceAsync(experience);
        return id;
    }
}