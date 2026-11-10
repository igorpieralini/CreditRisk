using CreditRisk.RiskDecision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence.Configurations;

public class DecisionHistoryConfiguration : IEntityTypeConfiguration<DecisionHistory>
{
    public void Configure(EntityTypeBuilder<DecisionHistory> builder)
    {
        builder.ToTable("decision_histories");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .ValueGeneratedNever();

        builder.Property(h => h.CreditDecisionId)
            .IsRequired();

        builder.Property(h => h.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(h => h.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(h => h.CreatedAt)
            .IsRequired();

        builder.HasOne(h => h.CreditDecision)
            .WithMany(d => d.Histories)
            .HasForeignKey(h => h.CreditDecisionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(h => h.CreditDecisionId);
        builder.HasIndex(h => h.CreatedAt);
    }
}