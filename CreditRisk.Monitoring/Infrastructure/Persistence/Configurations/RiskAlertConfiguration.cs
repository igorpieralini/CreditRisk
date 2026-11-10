using CreditRisk.Monitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.Monitoring.Infrastructure.Persistence.Configurations;

public class RiskAlertConfiguration : IEntityTypeConfiguration<RiskAlert>
{
    public void Configure(EntityTypeBuilder<RiskAlert> builder)
    {
        builder.ToTable("risk_alerts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.CreditMonitoringId)
            .IsRequired();

        builder.Property(a => a.Level)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(a => a.Title)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.Property(a => a.ResolvedAt);

        builder.HasOne(a => a.CreditMonitoring)
            .WithMany(m => m.Alerts)
            .HasForeignKey(a => a.CreditMonitoringId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.CreditMonitoringId);

        builder.HasIndex(a => a.Level);

        builder.HasIndex(a => a.Status);

        builder.HasIndex(a => a.CreatedAt);
    }
}