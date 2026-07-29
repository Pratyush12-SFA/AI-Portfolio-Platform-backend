using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeConfiguration : IEntityTypeConfiguration<Domain.Entites.Resume.Resume>
{
    public void Configure(EntityTypeBuilder<Domain.Entites.Resume.Resume> builder)
    {
        builder.ToTable("Resumes", "Resume");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.TargetJobTitle).HasMaxLength(150);
        builder.Property(r => r.Summary).HasMaxLength(2000);

        builder.Property(r => r.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(r => r.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(r => r.UpdatedBy).HasMaxLength(100);
        builder.Property(r => r.UpdatedFromIp).HasMaxLength(50);
        builder.Property(r => r.DeletedBy).HasMaxLength(100);
        builder.Property(r => r.DeletedFromIp).HasMaxLength(50);
        builder.Property(r => r.RowVersion).IsRowVersion();

        builder.HasIndex(r => r.UserId)
            .HasDatabaseName("IX_Resumes_UserId");

        builder.HasIndex(r => new { r.UserId, r.IsPrimary })
            .HasDatabaseName("IX_Resumes_UserId_IsPrimary");

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}