using CreditRisk.Monitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Monitoring.Infrastructure.Persistence;

public class MonitoringDbContext : DbContext
{
    public MonitoringDbContext(
        DbContextOptions<MonitoringDbContext> options)
        : base(options)
    {
    }

    public DbSet<CreditMonitoring> CreditMonitorings =>
        Set<CreditMonitoring>();

    public DbSet<RiskAlert> RiskAlerts =>
        Set<RiskAlert>();

    public DbSet<MonitoringEvent> MonitoringEvents =>
        Set<MonitoringEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MonitoringDbContext).Assembly);
    }
}