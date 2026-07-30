using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertExperience;

public sealed record UpsertExperienceCommand(
    long? Id,
    [property: JsonPropertyName("Company")] string CompanyName,
    [property: JsonPropertyName("Position")] string JobTitle,
    string? Location,
    string? StartDate,
    string? EndDate,
    bool IsCurrent,
    string? Description,
    string? Responsibilities,
    string? EmploymentType,
    int OrderIndex
) : IRequest<Result<long>>;