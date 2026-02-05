using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(product => product.ImageUrl)
            .HasColumnName("image_url");

        builder.Property(product => product.IsActive)
            .HasColumnName("is_active");

        builder.Property(product => product.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(product => product.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(product => product.ProductTypeId)
            .HasColumnName("product_type_id");

        builder.HasIndex(product => product.ProductTypeId);

        builder.HasMany(product => product.Variants)
            .WithOne(variant => variant.Product)
            .HasForeignKey(variant => variant.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(product => product.ExpertProducts)
            .WithOne(expertProduct => expertProduct.Product)
            .HasForeignKey(expertProduct => expertProduct.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(product => product.RecordedContents)
            .WithOne(content => content.Product)
            .HasForeignKey(content => content.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(product => product.Events)
            .WithOne(@event => @event.Product)
            .HasForeignKey(@event => @event.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
