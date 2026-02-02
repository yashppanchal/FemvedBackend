using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("notification_templates");
        builder.HasKey(template => template.Id);

        builder.Property(template => template.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(template => template.TitleTemplate)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(template => template.BodyTemplate)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(template => template.Channel)
            .IsRequired();

        builder.HasIndex(template => template.Name)
            .IsUnique();
    }
}
