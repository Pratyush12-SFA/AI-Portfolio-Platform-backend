using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertAchievement;

public sealed record UpsertAchievementCommand(
    long? Id,
    string Title,
    string? Description,
    [property: JsonPropertyName("Date")] string? AchievedDate,
    int OrderIndex
) : IRequest<Result<long>>;