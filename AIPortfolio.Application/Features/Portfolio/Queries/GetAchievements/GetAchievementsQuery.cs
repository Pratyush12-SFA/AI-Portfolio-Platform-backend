using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetAchievements;

public sealed record GetAchievementsQuery : IRequest<Result<IEnumerable<ResumeAchievement>>>;