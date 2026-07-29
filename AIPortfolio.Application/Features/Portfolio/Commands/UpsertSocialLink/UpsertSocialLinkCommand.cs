using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertSocialLink;

public sealed record UpsertSocialLinkCommand(
    long? Id,
    string Platform,
    string Url,
    int OrderIndex
) : IRequest<Result<long>>;