using AIPortfolio.Domain.Entites.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Jobs;

internal sealed class InterviewRoundConfiguration : IEntityTypeConfiguration<InterviewRound>
{
    public void Configure(EntityTypeBuilder<InterviewRound> builder)
    {
        builder.ToTable("InterviewRounds", "Jobs");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoundType).IsRequired().HasMaxLength(100);
        builder.Property(r => r.InterviewerName).HasMaxLength(150);
        builder.Property(r => r.Notes).HasMaxLength(2000);
        builder.Property(r => r.Outcome).HasMaxLength(50);

        builder.Property(r => r.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(r => r.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(r => r.UpdatedBy).HasMaxLength(100);
        builder.Property(r => r.UpdatedFromIp).HasMaxLength(50);
        builder.Property(r => r.DeletedBy).HasMaxLength(100);
        builder.Property(r => r.DeletedFromIp).HasMaxLength(50);
        builder.Property(r => r.RowVersion).IsRowVersion();

        builder.HasOne(r => r.JobApplication)
            .WithMany(j => j.InterviewRounds)
            .HasForeignKey(r => r.JobApplicationId)
            .HasConstraintName("FK_InterviewRounds_JobApplications")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}