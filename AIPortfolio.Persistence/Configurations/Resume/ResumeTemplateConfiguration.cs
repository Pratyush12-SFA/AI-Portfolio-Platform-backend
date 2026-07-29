using AIPortfolio.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.Resume;

internal sealed class ResumeTemplateConfiguration : IEntityTypeConfiguration<ResumeTemplate>
{
    public void Configure(EntityTypeBuilder<ResumeTemplate> builder)
    {
        builder.ToTable("ResumeTemplates", "Resume");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
        builder.Property(t => t.ThumbnailUrl).HasMaxLength(500);
        builder.Property(t => t.ConfigJson).HasColumnType("nvarchar(max)");

        builder.Property(t => t.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(t => t.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(t => t.UpdatedBy).HasMaxLength(100);
        builder.Property(t => t.UpdatedFromIp).HasMaxLength(50);
        builder.Property(t => t.DeletedBy).HasMaxLength(100);
        builder.Property(t => t.DeletedFromIp).HasMaxLength(50);
        builder.Property(t => t.RowVersion).IsRowVersion();

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}