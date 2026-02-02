using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("refunds");
        builder.HasKey(refund => refund.Id);

        builder.Property(refund => refund.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(refund => refund.Status)
            .IsRequired();

        builder.HasIndex(refund => refund.PaymentId);

        builder.HasOne(refund => refund.Payment)
            .WithMany(payment => payment.Refunds)
            .HasForeignKey(refund => refund.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
