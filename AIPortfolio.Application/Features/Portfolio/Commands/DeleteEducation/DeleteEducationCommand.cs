using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteEducation;

public sealed record DeleteEducationCommand(long Id) : IRequest<Result>;