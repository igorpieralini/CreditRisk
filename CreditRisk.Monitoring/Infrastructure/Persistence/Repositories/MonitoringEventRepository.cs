using CreditRisk.Monitoring.Domain.Entities;
using CreditRisk.Monitoring.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Monitoring.Infrastructure.Persistence.Repositories;

public class MonitoringEventRepository : IMonitoringEventRepository
{
    private readonly MonitoringDbContext _context;

    public MonitoringEventRepository(MonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<MonitoringEvent?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.MonitoringEvents
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<MonitoringEvent>> GetByCreditMonitoringIdAsync(
        Guid creditMonitoringId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MonitoringEvents
            .AsNoTracking()
            .Where(e => e.CreditMonitoringId == creditMonitoringId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MonitoringEvent>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.MonitoringEvents
            .AsNoTracking()
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        MonitoringEvent monitoringEvent,
        CancellationToken cancellationToken = default)
    {
        await _context.MonitoringEvents.AddAsync(
            monitoringEvent,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}