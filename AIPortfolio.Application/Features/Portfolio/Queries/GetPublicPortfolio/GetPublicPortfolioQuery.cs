using AIPortfolio.Application.DTOs.Portfolio;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetPublicPortfolio;

public sealed record GetPublicPortfolioQuery(string Slug) : IRequest<Result<PublicPortfolioResponse>>;