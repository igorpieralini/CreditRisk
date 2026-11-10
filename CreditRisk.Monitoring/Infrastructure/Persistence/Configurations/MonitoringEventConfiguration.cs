using CreditRisk.Monitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.Monitoring.Infrastructure.Persistence.Configurations;

public class MonitoringEventConfiguration : IEntityTypeConfiguration<MonitoringEvent>
{
    public void Configure(EntityTypeBuilder<MonitoringEvent> builder)
    {
        builder.ToTable("monitoring_events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.CreditMonitoringId)
            .IsRequired();

        builder.Property(e => e.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.HasOne(e => e.CreditMonitoring)
            .WithMany(m => m.Events)
            .HasForeignKey(e => e.CreditMonitoringId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.CreditMonitoringId);

        builder.HasIndex(e => e.Type);

        builder.HasIndex(e => e.CreatedAt);
    }
}