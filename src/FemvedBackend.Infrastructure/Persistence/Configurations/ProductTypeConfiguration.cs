using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.ToTable("product_types");
        builder.HasKey(productType => productType.Id);

        builder.Property(productType => productType.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(productType => productType.Name)
            .IsUnique();

        builder.HasMany(productType => productType.Products)
            .WithOne(product => product.ProductType)
            .HasForeignKey(product => product.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
