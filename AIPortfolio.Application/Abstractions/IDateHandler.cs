namespace AIPortfolio.Application.Abstractions;

public interface IDateHandler
{
    public DateTime LocalNow { get; }
    public DateTime UtcNow { get; }
}