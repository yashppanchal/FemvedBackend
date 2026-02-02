using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class PaymentGatewayConfiguration : IEntityTypeConfiguration<PaymentGateway>
{
    public void Configure(EntityTypeBuilder<PaymentGateway> builder)
    {
        builder.ToTable("payment_gateways");
        builder.HasKey(gateway => gateway.Id);

        builder.Property(gateway => gateway.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(gateway => gateway.Type)
            .IsRequired();

        builder.Property(gateway => gateway.IsActive)
            .IsRequired();

        builder.HasIndex(gateway => gateway.Name)
            .IsUnique();
    }
}
