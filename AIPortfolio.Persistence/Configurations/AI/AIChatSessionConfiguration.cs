using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.AI;

internal sealed class AIChatSessionConfiguration : IEntityTypeConfiguration<AIChatSession>
{
    public void Configure(EntityTypeBuilder<AIChatSession> builder)
    {
        builder.ToTable("AIChatSessions", "AI");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Context).HasMaxLength(1000);

        builder.Property(s => s.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(s => s.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
        builder.Property(s => s.UpdatedFromIp).HasMaxLength(50);
        builder.Property(s => s.DeletedBy).HasMaxLength(100);
        builder.Property(s => s.DeletedFromIp).HasMaxLength(50);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .HasConstraintName("FK_AIChatSessions_Users")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}