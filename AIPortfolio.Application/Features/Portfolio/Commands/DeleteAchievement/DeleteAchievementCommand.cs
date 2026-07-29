using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteAchievement;

public sealed record DeleteAchievementCommand(long Id) : IRequest<Result>;