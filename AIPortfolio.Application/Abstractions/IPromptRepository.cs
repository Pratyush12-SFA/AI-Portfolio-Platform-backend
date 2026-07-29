using AIPortfolio.Domain.Entites;

namespace AIPortfolio.Application.Abstractions;

public interface IPromptRepository
{
    Task<PromptTemplate?> GetActiveTemplateByFeatureAsync(string feature);
    Task<long> CreateTemplateAsync(PromptTemplate template);
}