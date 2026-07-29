using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetLanguages;

public sealed record GetLanguagesQuery : IRequest<Result<IEnumerable<ResumeLanguage>>>;