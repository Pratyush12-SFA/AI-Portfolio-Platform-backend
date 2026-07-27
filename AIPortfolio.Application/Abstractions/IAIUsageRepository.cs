using AIPortfolio.Domain.Entites;
using System.Threading.Tasks;

namespace AIPortfolio.Application.Abstractions;

public interface IAIUsageRepository
{
    Task<long> CreateAsync(AIUsage usage);
}
