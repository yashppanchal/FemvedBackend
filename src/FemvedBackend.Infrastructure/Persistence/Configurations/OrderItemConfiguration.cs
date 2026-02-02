using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(item => item.Id);

        builder.Property(item => item.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.HasIndex(item => item.OrderId);
        builder.HasIndex(item => item.ProductVariantId);

        builder.HasOne(item => item.Order)
            .WithMany(order => order.Items)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.ProductVariant)
            .WithMany()
            .HasForeignKey(item => item.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
