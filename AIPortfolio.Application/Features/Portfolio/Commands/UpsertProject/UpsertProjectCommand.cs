using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertProject;

public sealed record UpsertProjectCommand(
    long? Id,
    string Title,
    string? Description,
    string? TechStack,
    string? ProjectUrl,
    string? GithubUrl,
    DateTime? StartDate,
    DateTime? EndDate,
    string? Role,
    int OrderIndex,
    string? ThumbnailUrl
) : IRequest<Result<long>>;