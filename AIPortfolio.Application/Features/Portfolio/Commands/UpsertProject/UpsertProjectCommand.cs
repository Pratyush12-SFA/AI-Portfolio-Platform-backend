using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertProject;

public sealed record UpsertProjectCommand(
    long? Id,
    string Title,
    string? Description,
    [property: JsonPropertyName("Technologies")] string? TechStack,
    [property: JsonPropertyName("Url")] string? ProjectUrl,
    string? GithubUrl,
    string? StartDate,
    string? EndDate,
    string? Role,
    int OrderIndex,
    string? ThumbnailUrl
) : IRequest<Result<long>>;