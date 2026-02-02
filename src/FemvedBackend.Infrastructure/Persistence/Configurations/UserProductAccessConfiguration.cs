using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class UserProductAccessConfiguration : IEntityTypeConfiguration<UserProductAccess>
{
    public void Configure(EntityTypeBuilder<UserProductAccess> builder)
    {
        builder.ToTable("user_product_access");
        builder.HasKey(access => access.Id);

        builder.Property(access => access.GrantedAt)
            .IsRequired();

        builder.HasIndex(access => new { access.UserId, access.ProductId })
            .IsUnique();

        builder.HasOne(access => access.User)
            .WithMany(user => user.ProductAccesses)
            .HasForeignKey(access => access.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(access => access.Product)
            .WithMany()
            .HasForeignKey(access => access.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
