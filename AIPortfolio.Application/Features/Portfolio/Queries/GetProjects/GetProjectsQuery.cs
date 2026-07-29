using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetProjects;

public sealed record GetProjectsQuery : IRequest<Result<IEnumerable<ResumeProject>>>;