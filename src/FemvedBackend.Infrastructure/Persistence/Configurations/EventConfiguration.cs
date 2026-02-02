using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FemvedBackend.Infrastructure.Persistence.Configurations;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");
        builder.HasKey(@event => @event.Id);

        builder.Property(@event => @event.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(@event => @event.ProductId);
        builder.HasIndex(@event => @event.ExpertId);

        builder.HasOne(@event => @event.Product)
            .WithMany(product => product.Events)
            .HasForeignKey(@event => @event.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(@event => @event.Expert)
            .WithMany(expert => expert.Events)
            .HasForeignKey(@event => @event.ExpertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
