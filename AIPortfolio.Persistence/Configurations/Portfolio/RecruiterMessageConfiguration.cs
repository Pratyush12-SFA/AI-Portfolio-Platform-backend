using AIPortfolio.Domain.Entites.Portfolio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Portfolio;

internal sealed class RecruiterMessageConfiguration : IEntityTypeConfiguration<RecruiterMessage>
{
    public void Configure(EntityTypeBuilder<RecruiterMessage> builder)
    {
        builder.ToTable("RecruiterMessages", "Portfolio");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.SenderName).IsRequired().HasMaxLength(150);
        builder.Property(m => m.SenderEmail).IsRequired().HasMaxLength(255);
        builder.Property(m => m.SenderCompany).HasMaxLength(150);
        builder.Property(m => m.Message).IsRequired().HasColumnType("nvarchar(max)");

        builder.Property(m => m.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(m => m.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(m => m.UpdatedBy).HasMaxLength(100);
        builder.Property(m => m.UpdatedFromIp).HasMaxLength(50);
        builder.Property(m => m.DeletedBy).HasMaxLength(100);
        builder.Property(m => m.DeletedFromIp).HasMaxLength(50);
        builder.Property(m => m.RowVersion).IsRowVersion();

        builder.HasOne(m => m.Portfolio)
            .WithMany(p => p.RecruiterMessages)
            .HasForeignKey(m => m.PortfolioId)
            .HasConstraintName("FK_RecruiterMessages_Portfolios")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}