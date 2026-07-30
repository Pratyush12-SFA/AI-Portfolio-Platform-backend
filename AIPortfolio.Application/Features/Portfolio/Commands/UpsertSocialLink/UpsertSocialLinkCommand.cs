using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSocialLink;

public sealed record UpsertSocialLinkCommand(
    long? Id,
    [property: JsonPropertyName("PlatformName")]
    string Platform,
    string Url,
    int OrderIndex
) : IRequest<Result<long>>;