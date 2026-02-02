using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupons");
        builder.HasKey(coupon => coupon.Id);

        builder.Property(coupon => coupon.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(coupon => coupon.DiscountType)
            .IsRequired();

        builder.Property(coupon => coupon.DiscountValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(coupon => coupon.ValidFrom)
            .IsRequired();

        builder.Property(coupon => coupon.ValidTo)
            .IsRequired();

        builder.HasIndex(coupon => coupon.Code)
            .IsUnique();
    }
}
