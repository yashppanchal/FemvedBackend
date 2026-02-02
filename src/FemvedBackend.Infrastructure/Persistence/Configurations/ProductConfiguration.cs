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

        builder.Property(product => product.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasMaxLength(2000);

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
