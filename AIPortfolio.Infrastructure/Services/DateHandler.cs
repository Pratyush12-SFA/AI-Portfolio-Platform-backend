using AIPortfolio.Application.Abstraction;

namespace AIPortfolio.Infrastructure.Services;

public sealed class DateHandler : IDateHandler
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime LocalNow => DateTime.Now;
}