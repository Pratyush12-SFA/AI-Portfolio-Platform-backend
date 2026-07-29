using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteExperience;

public sealed record DeleteExperienceCommand(long Id) : IRequest<Result>;