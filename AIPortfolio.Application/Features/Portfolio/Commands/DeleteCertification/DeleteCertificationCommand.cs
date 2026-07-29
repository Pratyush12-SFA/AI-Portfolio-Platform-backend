using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteCertification;

public sealed record DeleteCertificationCommand(long Id) : IRequest<Result>;