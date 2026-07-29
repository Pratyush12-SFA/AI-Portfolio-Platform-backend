using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeSkillConfiguration : IEntityTypeConfiguration<ResumeSkill>
{
    public void Configure(EntityTypeBuilder<ResumeSkill> builder)
    {
        builder.ToTable("ResumeSkills", "Resume");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Category).HasMaxLength(100);
        builder.Property(s => s.ProficiencyLevel).HasMaxLength(50);

        builder.Property(s => s.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(s => s.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
        builder.Property(s => s.UpdatedFromIp).HasMaxLength(50);
        builder.Property(s => s.DeletedBy).HasMaxLength(100);
        builder.Property(s => s.DeletedFromIp).HasMaxLength(50);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.HasOne(s => s.Resume)
            .WithMany(r => r.Skills)
            .HasForeignKey(s => s.ResumeId)
            .HasConstraintName("FK_ResumeSkills_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}