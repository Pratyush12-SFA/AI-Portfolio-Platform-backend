using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertLanguage;

public sealed record UpsertLanguageCommand(
    long? Id,
    string Name,
    string? ProficiencyLevel,
    int OrderIndex
) : IRequest<Result<long>>;