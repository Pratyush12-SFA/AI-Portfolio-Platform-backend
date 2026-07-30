using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertEducation;

public sealed record UpsertEducationCommand(
    long? Id,
    string Institution,
    string Degree,
    string? FieldOfStudy,
    string? StartDate,
    string? EndDate,
    bool IsCurrent,
    string? Grade,
    string? Description,
    int OrderIndex
) : IRequest<Result<long>>;