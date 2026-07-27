using AIPortfolio.Domain.Common;

namespace AIPortfolio.Domain.Entites;

public sealed class Skill : AuditEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public byte ProficiencyPercentage { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
