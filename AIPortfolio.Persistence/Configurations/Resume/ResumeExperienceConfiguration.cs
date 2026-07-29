using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeExperienceConfiguration : IEntityTypeConfiguration<ResumeExperience>
{
    public void Configure(EntityTypeBuilder<ResumeExperience> builder)
    {
        builder.ToTable("ResumeExperiences", "Resume");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
        builder.Property(e => e.JobTitle).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Location).HasMaxLength(150);
        builder.Property(e => e.Description).HasColumnType("nvarchar(max)");
        builder.Property(e => e.Responsibilities).HasColumnType("nvarchar(max)");

        builder.Property(e => e.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(e => e.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedFromIp).HasMaxLength(50);
        builder.Property(e => e.DeletedBy).HasMaxLength(100);
        builder.Property(e => e.DeletedFromIp).HasMaxLength(50);
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.Ignore(e => e.Company);
        builder.Ignore(e => e.Position);
        builder.Ignore(e => e.Designation);
        builder.Ignore(e => e.DisplayOrder);

        builder.HasOne(e => e.Resume)
            .WithMany(r => r.Experiences)
            .HasForeignKey(e => e.ResumeId)
            .HasConstraintName("FK_ResumeExperiences_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
