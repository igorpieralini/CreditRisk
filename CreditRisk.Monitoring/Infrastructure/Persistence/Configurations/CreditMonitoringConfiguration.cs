using CreditRisk.Monitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.Monitoring.Infrastructure.Persistence.Configurations;

public class CreditMonitoringConfiguration : IEntityTypeConfiguration<CreditMonitoring>
{
    public void Configure(EntityTypeBuilder<CreditMonitoring> builder)
    {
        builder.ToTable("credit_monitorings");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.CreditOperationId)
            .IsRequired();

        builder.Property(m => m.RiskScore)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(m => m.GuaranteeCoveragePercentage)
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(m => m.LimitUtilizationPercentage)
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.HasMany(m => m.Alerts)
            .WithOne(a => a.CreditMonitoring)
            .HasForeignKey(a => a.CreditMonitoringId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Events)
            .WithOne(e => e.CreditMonitoring)
            .HasForeignKey(e => e.CreditMonitoringId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => m.CreditOperationId)
            .IsUnique();

        builder.HasIndex(m => m.RiskScore);
    }
}