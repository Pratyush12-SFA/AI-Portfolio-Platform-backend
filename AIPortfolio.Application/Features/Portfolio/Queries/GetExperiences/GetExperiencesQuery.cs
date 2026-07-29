using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetExperiences;

public sealed record GetExperiencesQuery : IRequest<Result<IEnumerable<ResumeExperience>>>;