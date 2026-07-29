using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Identity;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "Identity");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(512);

        builder.Property(u => u.GoogleId)
            .HasMaxLength(100);

        builder.Property(u => u.ProfilePictureUrl)
            .HasMaxLength(1000);

        builder.Property(u => u.ResetPasswordToken)
            .HasMaxLength(512);

        builder.Property(u => u.VerificationToken)
            .HasMaxLength(512);

        builder.Property(u => u.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(u => u.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(u => u.UpdatedBy).HasMaxLength(100);
        builder.Property(u => u.UpdatedFromIp).HasMaxLength(50);
        builder.Property(u => u.DeletedBy).HasMaxLength(100);
        builder.Property(u => u.DeletedFromIp).HasMaxLength(50);
        builder.Property(u => u.RowVersion).IsRowVersion();

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("UX_Users_Email");

        builder.HasIndex(u => u.GoogleId)
            .HasDatabaseName("IX_Users_GoogleId");

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
