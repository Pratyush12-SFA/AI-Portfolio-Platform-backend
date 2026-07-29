using AIPortfolio.Domain.Entites.Resume;
using Ardalis.Result;
using MediatR;

namespace AIPortfolio.Application.Features.Portfolio.Queries.GetCertifications;

public sealed record GetCertificationsQuery : IRequest<Result<IEnumerable<ResumeCertification>>>;