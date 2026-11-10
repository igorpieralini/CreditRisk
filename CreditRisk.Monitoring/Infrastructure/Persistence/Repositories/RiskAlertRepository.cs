using CreditRisk.Monitoring.Domain.Entities;
using CreditRisk.Monitoring.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Monitoring.Infrastructure.Persistence.Repositories;

public class RiskAlertRepository : IRiskAlertRepository
{
    private readonly MonitoringDbContext _context;

    public RiskAlertRepository(MonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<RiskAlert?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.RiskAlerts
            .AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<RiskAlert>> GetByCreditMonitoringIdAsync(
        Guid creditMonitoringId,
        CancellationToken cancellationToken = default)
    {
        return await _context.RiskAlerts
            .AsNoTracking()
            .Where(a => a.CreditMonitoringId == creditMonitoringId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RiskAlert>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.RiskAlerts
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        RiskAlert alert,
        CancellationToken cancellationToken = default)
    {
        await _context.RiskAlerts.AddAsync(
            alert,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        RiskAlert alert,
        CancellationToken cancellationToken = default)
    {
        _context.RiskAlerts.Update(alert);

        await _context.SaveChangesAsync(cancellationToken);
    }
}