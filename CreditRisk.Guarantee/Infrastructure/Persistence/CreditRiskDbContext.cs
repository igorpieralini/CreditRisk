using CreditRisk.Guarantee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GuaranteeEntity = global::CreditRisk.Guarantee.Domain.Entities.Guarantee;

namespace CreditRisk.Guarantee.Infrastructure.Persistence;

public class CreditRiskDbContext : DbContext
{
    public CreditRiskDbContext(DbContextOptions<CreditRiskDbContext> options)
        : base(options)
    {
    }

    public DbSet<GuaranteeEntity> Guarantees => Set<GuaranteeEntity>();

    public DbSet<GuaranteeValuation> GuaranteeValuations =>
        Set<GuaranteeValuation>();

    public DbSet<GuaranteeOperation> GuaranteeOperations =>
        Set<GuaranteeOperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CreditRiskDbContext).Assembly
        );
    }
}