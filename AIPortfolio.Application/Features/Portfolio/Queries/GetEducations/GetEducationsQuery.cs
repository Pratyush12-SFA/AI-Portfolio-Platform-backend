using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetEducations;

public sealed record GetEducationsQuery : IRequest<Result<IEnumerable<ResumeEducation>>>;