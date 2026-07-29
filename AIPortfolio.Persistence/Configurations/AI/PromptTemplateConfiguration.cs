using AIPortfolio.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIPortfolio.Persistence.Configurations.AI;

internal sealed class PromptTemplateConfiguration : IEntityTypeConfiguration<PromptTemplate>
{
    public void Configure(EntityTypeBuilder<PromptTemplate> builder)
    {
        builder.ToTable("PromptTemplates", "AI");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Feature).IsRequired().HasMaxLength(100);
        builder.Property(p => p.SystemPrompt).IsRequired().HasColumnType("nvarchar(max)");

        builder.Property(p => p.CreatedBy).HasMaxLength(100).IsRequired();
        builder.Property(p => p.CreatedFromIp).HasMaxLength(50).IsRequired();
        builder.Property(p => p.UpdatedBy).HasMaxLength(100);
        builder.Property(p => p.UpdatedFromIp).HasMaxLength(50);
        builder.Property(p => p.DeletedBy).HasMaxLength(100);
        builder.Property(p => p.DeletedFromIp).HasMaxLength(50);
        builder.Property(p => p.RowVersion).IsRowVersion();

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}