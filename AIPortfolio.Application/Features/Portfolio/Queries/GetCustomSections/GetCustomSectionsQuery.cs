using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetCustomSections;

public sealed record GetCustomSectionsQuery : IRequest<Result<IEnumerable<ResumeCustomSection>>>;