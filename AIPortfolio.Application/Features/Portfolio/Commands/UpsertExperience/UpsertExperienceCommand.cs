using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertExperience;

public sealed record UpsertExperienceCommand(
    long? Id,
    string CompanyName,
    string JobTitle,
    string? Location,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrent,
    string? Description,
    string? Responsibilities,
    string? EmploymentType,
    int OrderIndex
) : IRequest<Result<long>>;