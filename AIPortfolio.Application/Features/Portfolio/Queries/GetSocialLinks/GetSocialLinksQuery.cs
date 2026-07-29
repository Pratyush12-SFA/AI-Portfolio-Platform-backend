using AIPortfolio.Domain.Entites.Portfolio;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetSocialLinks;

public sealed record GetSocialLinksQuery : IRequest<Result<IEnumerable<PortfolioSocialLink>>>;