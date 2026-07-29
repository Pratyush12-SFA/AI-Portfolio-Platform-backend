using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeCustomSectionConfiguration : IEntityTypeConfiguration<ResumeCustomSection>
{
    public void Configure(EntityTypeBuilder<ResumeCustomSection> builder)
    {
        builder.ToTable("ResumeCustomSections", "Resume");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SectionTitle).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Content).HasColumnType("nvarchar(max)");

        builder.Property(s => s.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(s => s.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
        builder.Property(s => s.UpdatedFromIp).HasMaxLength(50);
        builder.Property(s => s.DeletedBy).HasMaxLength(100);
        builder.Property(s => s.DeletedFromIp).HasMaxLength(50);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.Ignore(s => s.Title);
        builder.Ignore(s => s.SectionName);
        builder.Ignore(s => s.DisplayOrder);

        builder.HasOne(s => s.Resume)
            .WithMany(r => r.CustomSections)
            .HasForeignKey(s => s.ResumeId)
            .HasConstraintName("FK_ResumeCustomSections_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
