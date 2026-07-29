using AIPortfolio.Domain.Entites.Portfolio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Portfolio;

internal sealed class PortfolioSocialLinkConfiguration : IEntityTypeConfiguration<PortfolioSocialLink>
{
    public void Configure(EntityTypeBuilder<PortfolioSocialLink> builder)
    {
        builder.ToTable("PortfolioSocialLinks", "Portfolio");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Platform).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Url).IsRequired().HasMaxLength(500);

        builder.Property(s => s.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(s => s.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
        builder.Property(s => s.UpdatedFromIp).HasMaxLength(50);
        builder.Property(s => s.DeletedBy).HasMaxLength(100);
        builder.Property(s => s.DeletedFromIp).HasMaxLength(50);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.HasOne(s => s.Portfolio)
            .WithMany(p => p.SocialLinks)
            .HasForeignKey(s => s.PortfolioId)
            .HasConstraintName("FK_PortfolioSocialLinks_Portfolios")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
