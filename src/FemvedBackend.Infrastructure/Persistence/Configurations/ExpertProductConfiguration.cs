using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class ExpertProductConfiguration : IEntityTypeConfiguration<ExpertProduct>
{
    public void Configure(EntityTypeBuilder<ExpertProduct> builder)
    {
        builder.ToTable("expert_products");
        builder.HasKey(expertProduct => expertProduct.Id);

        builder.Property(expertProduct => expertProduct.Id)
            .HasColumnName("id");

        builder.HasIndex(expertProduct => new { expertProduct.ProductId, expertProduct.ExpertId, expertProduct.DurationWeeks })
            .IsUnique();

        builder.Property(expertProduct => expertProduct.ProductId)
            .HasColumnName("product_id");

        builder.Property(expertProduct => expertProduct.ExpertId)
            .HasColumnName("expert_id");

        builder.Property(expertProduct => expertProduct.DurationWeeks)
            .HasColumnName("duration_weeks");

        builder.Property(expertProduct => expertProduct.OriginalPrice)
            .HasColumnName("original_price");

        builder.Property(expertProduct => expertProduct.DiscountPercentage)
            .HasColumnName("discount_percentage");

        builder.Property(expertProduct => expertProduct.FinalPrice)
            .HasColumnName("final_price");

        builder.Property(expertProduct => expertProduct.CurrencyCode)
            .HasColumnName("currency_code")
            .HasMaxLength(3);

        builder.Property(expertProduct => expertProduct.DisplayOrder)
            .HasColumnName("display_order");

        builder.Property(expertProduct => expertProduct.IsActive)
            .HasColumnName("is_active");

        builder.HasOne(expertProduct => expertProduct.Expert)
            .WithMany(expert => expert.ExpertProducts)
            .HasForeignKey(expertProduct => expertProduct.ExpertId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(expertProduct => expertProduct.Product)
            .WithMany(product => product.ExpertProducts)
            .HasForeignKey(expertProduct => expertProduct.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
