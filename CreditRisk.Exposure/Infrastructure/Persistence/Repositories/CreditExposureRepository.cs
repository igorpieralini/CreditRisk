
using Exposure.Domain.Entities;
using Exposure.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exposure.Infrastructure.Persistence.Repositories;

public class CreditExposureRepository : ICreditExposureRepository
{
    private readonly CreditRiskDbContext _context;

    public CreditExposureRepository(CreditRiskDbContext context)
    {
        _context = context;
    }

    public async Task<CreditExposure?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditExposures
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<CreditExposure?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditExposures
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.CreditOperationId == creditOperationId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<CreditExposure>> GetByCreditLimitIdAsync(
        Guid creditLimitId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditExposures
            .AsNoTracking()
            .Where(e => e.CreditLimitId == creditLimitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CreditExposure>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditExposures
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CreditExposure exposure,
        CancellationToken cancellationToken = default)
    {
        await _context.CreditExposures.AddAsync(
            exposure,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        CreditExposure exposure,
        CancellationToken cancellationToken = default)
    {
        _context.CreditExposures.Update(exposure);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
