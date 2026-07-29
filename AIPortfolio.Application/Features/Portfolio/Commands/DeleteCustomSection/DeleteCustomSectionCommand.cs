using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteCustomSection;

public sealed record DeleteCustomSectionCommand(long Id) : IRequest<Result>;