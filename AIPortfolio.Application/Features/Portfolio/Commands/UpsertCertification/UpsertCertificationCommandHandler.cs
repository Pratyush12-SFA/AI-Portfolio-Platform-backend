using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCertification;

internal sealed class UpsertCertificationCommandHandler(
    IPortfolioRepository repository,
    IUserInfoAccessor userInfoAccessor) : IRequestHandler<UpsertCertificationCommand, Result<long>>
{
    public async Task<Result<long>> Handle(UpsertCertificationCommand command, CancellationToken cancellationToken)
    {
        if (!userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var entity = new ResumeCertification
        {
            Id = command.Id ?? 0,
            Name = command.Name,
            IssuingOrganization = command.IssuingOrganization,
            IssueDate = command.IssueDate,
            ExpiryDate = command.ExpiryDate,
            CredentialId = command.CredentialId,
            CredentialUrl = command.CredentialUrl,
            OrderIndex = command.OrderIndex,
            CreatedBy = userInfoAccessor.Email ?? "system",
            CreatedFromIp = userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        var id = await repository.UpsertCertificationAsync(entity);
        return id;
    }
}