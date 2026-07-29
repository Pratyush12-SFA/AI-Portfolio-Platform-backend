using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeProjectConfiguration : IEntityTypeConfiguration<ResumeProject>
{
    public void Configure(EntityTypeBuilder<ResumeProject> builder)
    {
        builder.ToTable("ResumeProjects", "Resume");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).HasColumnType("nvarchar(max)");
        builder.Property(p => p.TechStack).HasMaxLength(500);
        builder.Property(p => p.ProjectUrl).HasMaxLength(500);
        builder.Property(p => p.GithubUrl).HasMaxLength(500);
        builder.Property(p => p.ThumbnailUrl).HasMaxLength(500);

        builder.Property(p => p.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(p => p.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(p => p.UpdatedBy).HasMaxLength(100);
        builder.Property(p => p.UpdatedFromIp).HasMaxLength(50);
        builder.Property(p => p.DeletedBy).HasMaxLength(100);
        builder.Property(p => p.DeletedFromIp).HasMaxLength(50);
        builder.Property(p => p.RowVersion).IsRowVersion();

        builder.Ignore(p => p.LiveDemoUrl);
        builder.Ignore(p => p.Technologies);
        builder.Ignore(p => p.Url);
        builder.Ignore(p => p.DisplayOrder);

        builder.HasOne(p => p.Resume)
            .WithMany(r => r.Projects)
            .HasForeignKey(p => p.ResumeId)
            .HasConstraintName("FK_ResumeProjects_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}