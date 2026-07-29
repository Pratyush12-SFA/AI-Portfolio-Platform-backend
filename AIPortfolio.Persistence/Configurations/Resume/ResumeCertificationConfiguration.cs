using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeCertificationConfiguration : IEntityTypeConfiguration<ResumeCertification>
{
    public void Configure(EntityTypeBuilder<ResumeCertification> builder)
    {
        builder.ToTable("ResumeCertifications", "Resume");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.IssuingOrganization).HasMaxLength(200);
        builder.Property(c => c.CredentialId).HasMaxLength(150);
        builder.Property(c => c.CredentialUrl).HasMaxLength(500);

        builder.Property(c => c.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(c => c.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(c => c.UpdatedBy).HasMaxLength(100);
        builder.Property(c => c.UpdatedFromIp).HasMaxLength(50);
        builder.Property(c => c.DeletedBy).HasMaxLength(100);
        builder.Property(c => c.DeletedFromIp).HasMaxLength(50);
        builder.Property(c => c.RowVersion).IsRowVersion();

        builder.Ignore(c => c.Issuer);
        builder.Ignore(c => c.DisplayOrder);
        builder.Ignore(c => c.ExpirationDate);

        builder.HasOne(c => c.Resume)
            .WithMany(r => r.Certifications)
            .HasForeignKey(c => c.ResumeId)
            .HasConstraintName("FK_ResumeCertifications_Resumes")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
