using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(order => order.Id);

        builder.Property(order => order.Status)
            .IsRequired();

        builder.Property(order => order.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(order => order.UserId);
        builder.HasIndex(order => order.Status);

        builder.HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
