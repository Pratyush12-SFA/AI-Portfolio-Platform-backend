using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCertification;

public sealed record UpsertCertificationCommand(
    long? Id,
    string Name,
    [property: JsonPropertyName("Issuer")] string? IssuingOrganization,
    string? IssueDate,
    [property: JsonPropertyName("ExpirationDate")]
    string? ExpiryDate,
    string? CredentialId,
    string? CredentialUrl,
    int OrderIndex
) : IRequest<Result<long>>;