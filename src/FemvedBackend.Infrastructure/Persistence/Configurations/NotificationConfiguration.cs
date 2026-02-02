using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(notification => notification.Id);

        builder.Property(notification => notification.Title)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(notification => notification.Message)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(notification => notification.Status)
            .IsRequired();

        builder.Property(notification => notification.CreatedAt)
            .IsRequired();

        builder.HasIndex(notification => notification.UserId);
        builder.HasIndex(notification => notification.Status);

        builder.HasOne(notification => notification.User)
            .WithMany(user => user.Notifications)
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(notification => notification.NotificationTemplate)
            .WithMany(template => template.Notifications)
            .HasForeignKey(notification => notification.NotificationTemplateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
