using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Identity;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", "Identity");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(r => r.RevokedBy)
            .HasMaxLength(100);

        builder.Property(r => r.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(r => r.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(r => r.UpdatedBy).HasMaxLength(100);
        builder.Property(r => r.UpdatedFromIp).HasMaxLength(50);
        builder.Property(r => r.DeletedBy).HasMaxLength(100);
        builder.Property(r => r.DeletedFromIp).HasMaxLength(50);
        builder.Property(r => r.RowVersion).IsRowVersion();

        builder.HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .HasConstraintName("FK_RefreshTokens_Users")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.Token)
            .HasDatabaseName("IX_RefreshTokens_Token");

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
