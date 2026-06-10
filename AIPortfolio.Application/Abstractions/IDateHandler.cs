namespace AIPortfolio.Application.Abstraction;

public interface IDateHandler
{
    public DateTime LocalNow { get; }
    public DateTime UtcNow { get; }
}