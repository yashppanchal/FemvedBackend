using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Status)
            .IsRequired();

        builder.Property(payment => payment.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(payment => payment.OrderId);
        builder.HasIndex(payment => payment.PaymentGatewayId);

        builder.HasOne(payment => payment.Order)
            .WithMany(order => order.Payments)
            .HasForeignKey(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(payment => payment.PaymentGateway)
            .WithMany(gateway => gateway.Payments)
            .HasForeignKey(payment => payment.PaymentGatewayId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
