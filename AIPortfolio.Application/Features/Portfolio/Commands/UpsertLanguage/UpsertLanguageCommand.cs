using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertLanguage;

public sealed record UpsertLanguageCommand(
    long? Id,
    string Name,
    [property: JsonPropertyName("Proficiency")] string? ProficiencyLevel,
    int OrderIndex
) : IRequest<Result<long>>;