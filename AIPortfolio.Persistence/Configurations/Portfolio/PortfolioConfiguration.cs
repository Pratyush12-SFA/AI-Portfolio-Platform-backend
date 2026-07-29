using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Portfolio;

internal sealed class PortfolioConfiguration : IEntityTypeConfiguration<Domain.Entites.Portfolio.Portfolio>
{
    public void Configure(EntityTypeBuilder<Domain.Entites.Portfolio.Portfolio> builder)
    {
        builder.ToTable("Portfolios", "Portfolio");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.CustomSlug).IsRequired().HasMaxLength(100);
        builder.Property(p => p.ProfileHeadline).HasMaxLength(250);
        builder.Property(p => p.ProfileSummary).HasColumnType("nvarchar(max)");
        builder.Property(p => p.ContactEmail).HasMaxLength(255);
        builder.Property(p => p.ContactPhone).HasMaxLength(50);
        builder.Property(p => p.Address).HasMaxLength(300);
        builder.Property(p => p.ProfilePictureUrl).HasMaxLength(1000);
        builder.Property(p => p.BannerPictureUrl).HasMaxLength(1000);

        builder.Property(p => p.ThemeName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.ThemeConfigJson).HasColumnType("nvarchar(max)");

        builder.Property(p => p.SEOTitle).HasMaxLength(200);
        builder.Property(p => p.SEODescription).HasMaxLength(500);
        builder.Property(p => p.SEOKeywords).HasMaxLength(500);

        builder.Property(p => p.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(p => p.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(p => p.UpdatedBy).HasMaxLength(100);
        builder.Property(p => p.UpdatedFromIp).HasMaxLength(50);
        builder.Property(p => p.DeletedBy).HasMaxLength(100);
        builder.Property(p => p.DeletedFromIp).HasMaxLength(50);
        builder.Property(p => p.RowVersion).IsRowVersion();

        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("UX_Portfolios_UserId");

        builder.HasIndex(p => p.CustomSlug)
            .IsUnique()
            .HasDatabaseName("UX_Portfolios_CustomSlug");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
