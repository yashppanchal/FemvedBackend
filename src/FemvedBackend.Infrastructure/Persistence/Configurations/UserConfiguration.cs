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

        builder.Property(user => user.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.CountryCode)
            .HasColumnName("country_code")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(user => user.MobileNumber)
            .HasColumnName("mobile_number")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(user => user.RoleId)
            .HasColumnName("role_id")
            .HasColumnType("smallint")
            .HasDefaultValue((short)3);

        builder.Property(user => user.WhatsappOptIn)
            .HasColumnName("whatsapp_opt_in");

        builder.Property(user => user.IsActive)
            .HasColumnName("is_active");

        builder.Property(user => user.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone");
            //.HasConversion<YourDateTimeOffsetConversion>(); // Keep DateTimeOffset mapping

        builder.Property(user => user.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");
            //.HasConversion<YourDateTimeOffsetConversion>(); // Keep DateTimeOffset mapping

        builder.Property(user => user.IsMobileVerified)
            .HasColumnName("is_mobile_verified")
            .HasDefaultValue(false);

        builder.Property(user => user.IsEmailVerified)
            .HasColumnName("is_email_verified")
            .HasDefaultValue(false);

        builder.Ignore(user => user.PasswordResetTokenHash);
        builder.Ignore(user => user.PasswordResetTokenExpiresAt);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => new { user.CountryCode, user.MobileNumber })
            .IsUnique()
            .HasFilter("mobile_number IS NOT NULL");
    }
}
