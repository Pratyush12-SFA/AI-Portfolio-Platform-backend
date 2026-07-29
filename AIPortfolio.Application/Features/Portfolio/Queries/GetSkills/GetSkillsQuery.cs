using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetSkills;

public sealed record GetSkillsQuery : IRequest<Result<IEnumerable<ResumeSkill>>>;