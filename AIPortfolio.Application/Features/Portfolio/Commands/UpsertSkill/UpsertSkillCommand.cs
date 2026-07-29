using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSkill;

public sealed record UpsertSkillCommand(
    long? Id,
    string Name,
    string? Category,
    string? ProficiencyLevel,
    int OrderIndex
) : IRequest<Result<long>>;