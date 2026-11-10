using CreditRisk.RiskDecision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence.Configurations;

public class DecisionPolicyConfiguration : IEntityTypeConfiguration<DecisionPolicy>
{
    public void Configure(EntityTypeBuilder<DecisionPolicy> builder)
    {
        builder.ToTable("decision_policies");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(p => p.MinimumRiskScore)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(p => p.MinimumGuaranteeCoverage)
            .HasPrecision(7, 2)
            .IsRequired();

        builder.Property(p => p.MaximumLimitUtilization)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.Name);

        builder.HasIndex(p => p.IsActive);
    }
}