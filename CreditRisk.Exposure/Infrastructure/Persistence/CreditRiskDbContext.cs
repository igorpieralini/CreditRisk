using Exposure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exposure.Infrastructure.Persistence;

public class CreditRiskDbContext : DbContext
{
    public CreditRiskDbContext(DbContextOptions<CreditRiskDbContext> options)
        : base(options)
    {
    }

    public DbSet<CreditExposure> CreditExposures => Set<CreditExposure>();

    public DbSet<CreditLimit> CreditLimits => Set<CreditLimit>();

    public DbSet<ExposureMovement> ExposureMovements => Set<ExposureMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CreditRiskDbContext).Assembly
        );
    }
}