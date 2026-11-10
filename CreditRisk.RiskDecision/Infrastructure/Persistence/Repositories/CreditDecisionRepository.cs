
using CreditRisk.RiskDecision.Domain.Entities;
using CreditRisk.RiskDecision.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence.Repositories;

public class CreditDecisionRepository : ICreditDecisionRepository
{
    private readonly RiskDecisionDbContext _context;

    public CreditDecisionRepository(RiskDecisionDbContext context)
    {
        _context = context;
    }

    public async Task<CreditDecision?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditDecisions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                d => d.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<CreditDecision>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditDecisions
            .AsNoTracking()
            .Where(d => d.CreditOperationId == creditOperationId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CreditDecision>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditDecisions
            .AsNoTracking()
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CreditDecision decision,
        CancellationToken cancellationToken = default)
    {
        await _context.CreditDecisions.AddAsync(
            decision,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        CreditDecision decision,
        CancellationToken cancellationToken = default)
    {
        _context.CreditDecisions.Update(decision);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
