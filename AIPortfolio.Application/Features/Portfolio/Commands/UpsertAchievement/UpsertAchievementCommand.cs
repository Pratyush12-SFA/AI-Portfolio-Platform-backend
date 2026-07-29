using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertAchievement;

public sealed record UpsertAchievementCommand(
    long? Id,
    string Title,
    string? Description,
    DateTime? AchievedDate,
    int OrderIndex
) : IRequest<Result<long>>;