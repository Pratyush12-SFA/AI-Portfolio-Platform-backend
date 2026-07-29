using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.DeleteLanguage;

public sealed record DeleteLanguageCommand(long Id) : IRequest<Result>;