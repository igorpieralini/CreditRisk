using CreditRisk.RiskDecision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence.Configurations;

public class CreditDecisionConfiguration : IEntityTypeConfiguration<CreditDecision>
{
    public void Configure(EntityTypeBuilder<CreditDecision> builder)
    {
        builder.ToTable("credit_decisions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever();

        builder.Property(d => d.CreditOperationId)
            .IsRequired();

        builder.Property(d => d.RequestedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(d => d.RiskScore)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(d => d.GuaranteeCoveragePercentage)
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(d => d.LimitUtilizationPercentage)
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.HasMany(d => d.Histories)
            .WithOne(h => h.CreditDecision)
            .HasForeignKey(h => h.CreditDecisionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => d.CreditOperationId);
        builder.HasIndex(d => d.Status);
        builder.HasIndex(d => d.CreatedAt);
    }
}