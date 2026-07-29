namespace AIPortfolio.Application.DTOs.AI;

public sealed class ATSResultDto
{
    public int Score { get; set; }
    public IEnumerable<string> MatchedKeywords { get; set; } = [];
    public IEnumerable<string> MissingKeywords { get; set; } = [];
    public IEnumerable<string> Recommendations { get; set; } = [];
}