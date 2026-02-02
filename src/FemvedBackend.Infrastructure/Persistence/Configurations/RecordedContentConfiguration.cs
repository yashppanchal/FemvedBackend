using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class RecordedContentConfiguration : IEntityTypeConfiguration<RecordedContent>
{
    public void Configure(EntityTypeBuilder<RecordedContent> builder)
    {
        builder.ToTable("recorded_contents");
        builder.HasKey(content => content.Id);

        builder.Property(content => content.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(content => content.ContentUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(content => content.ProductId);
    }
}
