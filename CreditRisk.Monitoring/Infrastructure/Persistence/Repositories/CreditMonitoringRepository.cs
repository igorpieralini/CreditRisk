
using CreditRisk.Monitoring.Domain.Entities;
using CreditRisk.Monitoring.Domain.Repositories;
using CreditRisk.Monitoring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Monitoring.Infrastructure.Persistence.Repositories;

public class CreditMonitoringRepository : ICreditMonitoringRepository
{
    private readonly MonitoringDbContext _context;

    public CreditMonitoringRepository(MonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<CreditMonitoring?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditMonitorings
            .AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.Id == id,
                cancellationToken);
    }

    public async Task<CreditMonitoring?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditMonitorings
            .AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.CreditOperationId == creditOperationId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<CreditMonitoring>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditMonitorings
            .AsNoTracking()
            .OrderByDescending(m => m.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CreditMonitoring monitoring,
        CancellationToken cancellationToken = default)
    {
        await _context.CreditMonitorings.AddAsync(
            monitoring,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        CreditMonitoring monitoring,
        CancellationToken cancellationToken = default)
    {
        _context.CreditMonitorings.Update(monitoring);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
