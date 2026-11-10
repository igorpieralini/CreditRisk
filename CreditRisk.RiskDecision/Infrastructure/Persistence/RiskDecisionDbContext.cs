using CreditRisk.RiskDecision.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence;

public class RiskDecisionDbContext : DbContext
{
    public RiskDecisionDbContext(
        DbContextOptions<RiskDecisionDbContext> options)
        : base(options)
    {
    }

    public DbSet<CreditDecision> CreditDecisions => Set<CreditDecision>();

    public DbSet<DecisionHistory> DecisionHistories => Set<DecisionHistory>();

    public DbSet<DecisionPolicy> DecisionPolicies => Set<DecisionPolicy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RiskDecisionDbContext).Assembly);
    }
}