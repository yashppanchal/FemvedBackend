using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
{
    public void Configure(EntityTypeBuilder<CouponUsage> builder)
    {
        builder.ToTable("coupon_usages");
        builder.HasKey(usage => usage.Id);

        builder.Property(usage => usage.UsedAt)
            .IsRequired();

        builder.HasIndex(usage => new { usage.CouponId, usage.UserId, usage.OrderId })
            .IsUnique();

        builder.HasOne(usage => usage.Coupon)
            .WithMany(coupon => coupon.Usages)
            .HasForeignKey(usage => usage.CouponId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(usage => usage.User)
            .WithMany(user => user.CouponUsages)
            .HasForeignKey(usage => usage.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(usage => usage.Order)
            .WithMany(order => order.CouponUsages)
            .HasForeignKey(usage => usage.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
