using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.AI;

internal sealed class AIUsageConfiguration : IEntityTypeConfiguration<AIUsage>
{
    public void Configure(EntityTypeBuilder<AIUsage> builder)
    {
        builder.ToTable("AIUsage", "AI");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FeatureName).IsRequired().HasMaxLength(100);

        builder.Property(u => u.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(u => u.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(u => u.UpdatedBy).HasMaxLength(100);
        builder.Property(u => u.UpdatedFromIp).HasMaxLength(50);
        builder.Property(u => u.DeletedBy).HasMaxLength(100);
        builder.Property(u => u.DeletedFromIp).HasMaxLength(50);
        builder.Property(u => u.RowVersion).IsRowVersion();

        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId)
            .HasConstraintName("FK_AIUsage_Users")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
