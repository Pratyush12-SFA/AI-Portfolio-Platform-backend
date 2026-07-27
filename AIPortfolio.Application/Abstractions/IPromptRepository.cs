using AIPortfolio.Domain.Entites;
using System.Threading.Tasks;

namespace AIPortfolio.Application.Abstractions;

public interface IPromptRepository
{
    Task<PromptTemplate?> GetActiveTemplateByFeatureAsync(string feature);
    Task<long> CreateTemplateAsync(PromptTemplate template);
}
