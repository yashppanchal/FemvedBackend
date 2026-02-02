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

        builder.HasIndex(expertProduct => new { expertProduct.ExpertId, expertProduct.ProductId })
            .IsUnique();

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
