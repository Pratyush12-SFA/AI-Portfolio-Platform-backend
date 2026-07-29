using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertCustomSection;

public sealed record UpsertCustomSectionCommand(
    long? Id,
    string SectionTitle,
    string Content,
    int OrderIndex
) : IRequest<Result<long>>;