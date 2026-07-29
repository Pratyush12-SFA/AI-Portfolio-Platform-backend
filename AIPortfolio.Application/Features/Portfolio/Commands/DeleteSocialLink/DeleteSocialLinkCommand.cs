using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteSocialLink;

public sealed record DeleteSocialLinkCommand(long Id) : IRequest<Result>;