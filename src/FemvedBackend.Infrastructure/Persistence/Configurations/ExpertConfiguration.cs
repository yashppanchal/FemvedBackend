using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class ExpertConfiguration : IEntityTypeConfiguration<Expert>
{
    public void Configure(EntityTypeBuilder<Expert> builder)
    {
        builder.ToTable("experts");
        builder.HasKey(expert => expert.UserId);

        builder.Property(expert => expert.UserId)
            .HasColumnName("user_id");

        builder.Property(expert => expert.Bio)
            .HasColumnName("bio")
            .HasMaxLength(2000);

        builder.Property(expert => expert.Specialization)
            .HasColumnName("specialization");

        builder.Property(expert => expert.Rating)
            .HasColumnName("rating");

        builder.Property(expert => expert.IsVerified)
            .HasColumnName("is_verified");

        builder.HasOne(expert => expert.User)
            .WithOne()
            .HasForeignKey<Expert>(expert => expert.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
