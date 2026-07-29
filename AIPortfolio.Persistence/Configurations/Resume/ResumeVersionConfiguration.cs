using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeVersionConfiguration : IEntityTypeConfiguration<ResumeVersion>
{
    public void Configure(EntityTypeBuilder<ResumeVersion> builder)
    {
        builder.ToTable("ResumeVersions", "Resume");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.JsonSnapshot).IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(v => v.Label).HasMaxLength(150);

        builder.Property(v => v.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(v => v.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(v => v.UpdatedBy).HasMaxLength(100);
        builder.Property(v => v.UpdatedFromIp).HasMaxLength(50);
        builder.Property(v => v.DeletedBy).HasMaxLength(100);
        builder.Property(v => v.DeletedFromIp).HasMaxLength(50);
        builder.Property(v => v.RowVersion).IsRowVersion();

        builder.HasOne(v => v.Resume)
            .WithMany(r => r.Versions)
            .HasForeignKey(v => v.ResumeId)
            .HasConstraintName("FK_ResumeVersions_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}