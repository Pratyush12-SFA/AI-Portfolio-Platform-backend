using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeLanguageConfiguration : IEntityTypeConfiguration<ResumeLanguage>
{
    public void Configure(EntityTypeBuilder<ResumeLanguage> builder)
    {
        builder.ToTable("ResumeLanguages", "Resume");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
        builder.Property(l => l.ProficiencyLevel).HasMaxLength(50);

        builder.Property(l => l.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(l => l.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(l => l.UpdatedBy).HasMaxLength(100);
        builder.Property(l => l.UpdatedFromIp).HasMaxLength(50);
        builder.Property(l => l.DeletedBy).HasMaxLength(100);
        builder.Property(l => l.DeletedFromIp).HasMaxLength(50);
        builder.Property(l => l.RowVersion).IsRowVersion();

        builder.Ignore(l => l.Proficiency);
        builder.Ignore(l => l.DisplayOrder);

        builder.HasOne(l => l.Resume)
            .WithMany(r => r.Languages)
            .HasForeignKey(l => l.ResumeId)
            .HasConstraintName("FK_ResumeLanguages_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}