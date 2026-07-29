using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Jobs;

public sealed class InterviewRound : BaseEntity
{
    public long JobApplicationId { get; set; }
    public int RoundNumber { get; set; }
    public string RoundType { get; set; } = string.Empty;   // Phone, Technical, HR, Final
    public DateTime? ScheduledAt { get; set; }
    public string? InterviewerName { get; set; }
    public string? Notes { get; set; }
    public string? Outcome { get; set; }    // Passed, Failed, Pending

    // Navigation
    public JobApplication JobApplication { get; set; } = null!;
}
