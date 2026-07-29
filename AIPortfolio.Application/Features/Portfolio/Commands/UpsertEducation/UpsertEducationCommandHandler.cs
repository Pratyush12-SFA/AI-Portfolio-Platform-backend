using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertEducation;

internal sealed class UpsertEducationCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertEducationCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertEducationCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var education = new ResumeEducation
        {
            Id = command.Id ?? 0,
            Institution = command.Institution,
            Degree = command.Degree,
            FieldOfStudy = command.FieldOfStudy,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            IsCurrent = command.IsCurrent,
            Grade = command.Grade,
            Description = command.Description,
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertEducationAsync(education);
        return id;
    }
}