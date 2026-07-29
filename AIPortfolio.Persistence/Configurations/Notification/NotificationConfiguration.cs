using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Notification;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Domain.Entites.Notification.Notification>
{
    public void Configure(EntityTypeBuilder<Domain.Entites.Notification.Notification> builder)
    {
        builder.ToTable("Notifications", "Notification");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Type).IsRequired().HasMaxLength(50);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
        builder.Property(n => n.Message).HasMaxLength(1000);
        builder.Property(n => n.ActionUrl).HasMaxLength(500);

        builder.Property(n => n.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(n => n.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(n => n.UpdatedBy).HasMaxLength(100);
        builder.Property(n => n.UpdatedFromIp).HasMaxLength(50);
        builder.Property(n => n.DeletedBy).HasMaxLength(100);
        builder.Property(n => n.DeletedFromIp).HasMaxLength(50);
        builder.Property(n => n.RowVersion).IsRowVersion();

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .HasConstraintName("FK_Notifications_Users")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.UserId)
            .HasDatabaseName("IX_Notifications_UserId");

        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}