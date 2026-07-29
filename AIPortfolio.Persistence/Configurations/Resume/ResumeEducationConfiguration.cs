using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeEducationConfiguration : IEntityTypeConfiguration<ResumeEducation>
{
    public void Configure(EntityTypeBuilder<ResumeEducation> builder)
    {
        builder.ToTable("ResumeEducations", "Resume");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Institution).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Degree).IsRequired().HasMaxLength(150);
        builder.Property(e => e.FieldOfStudy).HasMaxLength(150);
        builder.Property(e => e.Grade).HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(1000);

        builder.Property(e => e.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(e => e.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedFromIp).HasMaxLength(50);
        builder.Property(e => e.DeletedBy).HasMaxLength(100);
        builder.Property(e => e.DeletedFromIp).HasMaxLength(50);
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.HasOne(e => e.Resume)
            .WithMany(r => r.Educations)
            .HasForeignKey(e => e.ResumeId)
            .HasConstraintName("FK_ResumeEducations_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}