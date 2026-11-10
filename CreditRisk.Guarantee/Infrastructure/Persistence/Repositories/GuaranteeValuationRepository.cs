using CreditRisk.Guarantee.Domain.Entities;
using CreditRisk.Guarantee.Domain.Repositories;
using CreditRisk.Guarantee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Guarantee.Infrastructure.Persistence.Repositories;

public class GuaranteeValuationRepository : IGuaranteeValuationRepository
{
    private readonly CreditRiskDbContext _context;

    public GuaranteeValuationRepository(CreditRiskDbContext context)
    {
        _context = context;
    }

    public async Task<GuaranteeValuation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.GuaranteeValuations
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GuaranteeValuation>> GetByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.GuaranteeValuations
            .AsNoTracking()
            .Where(v => v.GuaranteeId == guaranteeId)
            .OrderByDescending(v => v.ValuationDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<GuaranteeValuation?> GetLatestByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.GuaranteeValuations
            .AsNoTracking()
            .Where(v => v.GuaranteeId == guaranteeId)
            .OrderByDescending(v => v.ValuationDate)
            .ThenByDescending(v => v.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(
        GuaranteeValuation valuation,
        CancellationToken cancellationToken = default)
    {
        await _context.GuaranteeValuations.AddAsync(
            valuation,
            cancellationToken
        );

        await _context.SaveChangesAsync(cancellationToken);
    }
}