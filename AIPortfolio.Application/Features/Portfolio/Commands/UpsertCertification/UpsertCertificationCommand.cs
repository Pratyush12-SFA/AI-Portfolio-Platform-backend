using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCertification;

public sealed record UpsertCertificationCommand(
    long? Id,
    string Name,
    string? IssuingOrganization,
    DateTime? IssueDate,
    DateTime? ExpiryDate,
    string? CredentialId,
    string? CredentialUrl,
    int OrderIndex
) : IRequest<Result<long>>;