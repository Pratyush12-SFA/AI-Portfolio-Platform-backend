using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeAchievementConfiguration : IEntityTypeConfiguration<ResumeAchievement>
{
    public void Configure(EntityTypeBuilder<ResumeAchievement> builder)
    {
        builder.ToTable("ResumeAchievements", "Resume");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Description).HasColumnType("nvarchar(max)");

        builder.Property(a => a.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(a => a.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(a => a.UpdatedBy).HasMaxLength(100);
        builder.Property(a => a.UpdatedFromIp).HasMaxLength(50);
        builder.Property(a => a.DeletedBy).HasMaxLength(100);
        builder.Property(a => a.DeletedFromIp).HasMaxLength(50);
        builder.Property(a => a.RowVersion).IsRowVersion();

        builder.Ignore(a => a.Date);
        builder.Ignore(a => a.DisplayOrder);

        builder.HasOne(a => a.Resume)
            .WithMany(r => r.Achievements)
            .HasForeignKey(a => a.ResumeId)
            .HasConstraintName("FK_ResumeAchievements_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
