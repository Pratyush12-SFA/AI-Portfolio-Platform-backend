using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Identity;

internal sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions", "Identity");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.DeviceName).HasMaxLength(200);
        builder.Property(s => s.IpAddress).HasMaxLength(50);
        builder.Property(s => s.UserAgent).HasMaxLength(500);

        builder.Property(s => s.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(s => s.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
        builder.Property(s => s.UpdatedFromIp).HasMaxLength(50);
        builder.Property(s => s.DeletedBy).HasMaxLength(100);
        builder.Property(s => s.DeletedFromIp).HasMaxLength(50);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.Ignore(s => s.DeviceDetails);
        builder.Ignore(s => s.DeviceType);
        builder.Ignore(s => s.IsCurrentActive);

        builder.HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId)
            .HasConstraintName("FK_UserSessions_Users")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.UserId)
            .HasDatabaseName("IX_UserSessions_UserId");

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}