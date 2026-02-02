using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");
        builder.HasKey(variant => variant.Id);

        builder.Property(variant => variant.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(variant => variant.Sku)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(variant => variant.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(variant => variant.Sku)
            .IsUnique();

        builder.HasIndex(variant => variant.ProductId);
    }
}
