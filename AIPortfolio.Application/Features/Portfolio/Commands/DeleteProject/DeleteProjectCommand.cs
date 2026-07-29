using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteProject;

public sealed record DeleteProjectCommand(long Id) : IRequest<Result>;