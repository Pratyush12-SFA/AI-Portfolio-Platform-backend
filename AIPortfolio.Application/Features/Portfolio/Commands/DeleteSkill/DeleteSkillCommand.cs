using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteSkill;

public sealed record DeleteSkillCommand(long Id) : IRequest<Result>;