using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.CountryCode)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(user => user.MobileNumber)
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(user => user.RoleId)
            .HasDefaultValue((short)3);

        builder.Property(user => user.IsMobileVerified)
            .HasDefaultValue(false);

        builder.Property(user => user.IsEmailVerified)
            .HasDefaultValue(false);

        builder.Property(user => user.PasswordResetTokenHash)
            .HasMaxLength(512);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => new { user.CountryCode, user.MobileNumber })
            .IsUnique()
            .HasFilter("mobile_number IS NOT NULL");
    }
}
