using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites.Jobs;

public sealed class JobApplication : BaseEntity
{
    public long UserId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? JobUrl { get; set; }
    public string? Location { get; set; }
    public string? JobType { get; set; }           // Full-time, Part-time, Contract
    public string? SalaryRange { get; set; }
    public string Status { get; set; } = "Saved";  // Saved, Applied, Screening, Interview, Offer, Rejected
    public DateTime? AppliedDate { get; set; }
    public string? Notes { get; set; }
    public string? JobDescription { get; set; }
    public int? ATSScore { get; set; }

    // Navigation
    public ICollection<InterviewRound> InterviewRounds { get; set; } = [];
}
