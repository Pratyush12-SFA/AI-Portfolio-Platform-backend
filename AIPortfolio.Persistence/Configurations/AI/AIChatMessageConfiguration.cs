using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.AI;

internal sealed class AIChatMessageConfiguration : IEntityTypeConfiguration<AIChatMessage>
{
    public void Configure(EntityTypeBuilder<AIChatMessage> builder)
    {
        builder.ToTable("AIChatMessages", "AI");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Role).IsRequired().HasMaxLength(50);
        builder.Property(m => m.Content).IsRequired().HasColumnType("nvarchar(max)");

        builder.Property(m => m.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(m => m.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(m => m.UpdatedBy).HasMaxLength(100);
        builder.Property(m => m.UpdatedFromIp).HasMaxLength(50);
        builder.Property(m => m.DeletedBy).HasMaxLength(100);
        builder.Property(m => m.DeletedFromIp).HasMaxLength(50);
        builder.Property(m => m.RowVersion).IsRowVersion();

        builder.HasOne(m => m.Session)
            .WithMany(s => s.Messages)
            .HasForeignKey(m => m.SessionId)
            .HasConstraintName("FK_AIChatMessages_AIChatSessions")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
