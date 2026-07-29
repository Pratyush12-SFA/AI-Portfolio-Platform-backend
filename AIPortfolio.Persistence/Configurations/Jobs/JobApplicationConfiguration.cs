using AIPortfolio.Domain.Entites.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Jobs;

internal sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications", "Jobs");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.JobTitle).IsRequired().HasMaxLength(200);
        builder.Property(j => j.CompanyName).IsRequired().HasMaxLength(200);
        builder.Property(j => j.JobUrl).HasMaxLength(500);
        builder.Property(j => j.Location).HasMaxLength(150);
        builder.Property(j => j.JobType).HasMaxLength(50);
        builder.Property(j => j.SalaryRange).HasMaxLength(100);
        builder.Property(j => j.Status).IsRequired().HasMaxLength(50);
        builder.Property(j => j.Notes).HasMaxLength(2000);
        builder.Property(j => j.JobDescription).HasColumnType("nvarchar(max)");

        builder.Property(j => j.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(j => j.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(j => j.UpdatedBy).HasMaxLength(100);
        builder.Property(j => j.UpdatedFromIp).HasMaxLength(50);
        builder.Property(j => j.DeletedBy).HasMaxLength(100);
        builder.Property(j => j.DeletedFromIp).HasMaxLength(50);
        builder.Property(j => j.RowVersion).IsRowVersion();

        builder.HasOne<Domain.Entites.User>()
            .WithMany()
            .HasForeignKey(j => j.UserId)
            .HasConstraintName("FK_JobApplications_Users")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(j => j.UserId)
            .HasDatabaseName("IX_JobApplications_UserId");

        builder.HasQueryFilter(j => !j.IsDeleted);
    }
}
