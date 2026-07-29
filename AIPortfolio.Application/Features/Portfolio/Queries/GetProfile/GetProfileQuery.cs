using Ardalis.Result;
using MediatR;
using PortfolioEntity = AIPortfolio.Domain.Entites.Portfolio.Portfolio;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetProfile;

public sealed record GetProfileQuery : IRequest<Result<PortfolioEntity>>;