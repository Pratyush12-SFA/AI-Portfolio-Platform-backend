using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCustomSection;

public sealed record UpsertCustomSectionCommand(
    long? Id,
    [property: JsonPropertyName("Title")] string SectionTitle,
    string Content,
    int OrderIndex
) : IRequest<Result<long>>;