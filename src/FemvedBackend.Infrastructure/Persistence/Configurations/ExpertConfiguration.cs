using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class ExpertConfiguration : IEntityTypeConfiguration<Expert>
{
    public void Configure(EntityTypeBuilder<Expert> builder)
    {
        builder.ToTable("experts");
        builder.HasKey(expert => expert.Id);

        builder.Property(expert => expert.DisplayName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(expert => expert.Bio)
            .HasMaxLength(2000);

        builder.HasIndex(expert => expert.UserId)
            .IsUnique();

        builder.HasOne(expert => expert.User)
            .WithOne()
            .HasForeignKey<Expert>(expert => expert.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
