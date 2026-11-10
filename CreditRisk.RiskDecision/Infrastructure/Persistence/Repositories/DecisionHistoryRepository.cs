using CreditRisk.RiskDecision.Domain.Entities;
using CreditRisk.RiskDecision.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence.Repositories;

public class DecisionHistoryRepository : IDecisionHistoryRepository
{
    private readonly RiskDecisionDbContext _context;

    public DecisionHistoryRepository(RiskDecisionDbContext context)
    {
        _context = context;
    }

    public async Task<DecisionHistory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.DecisionHistories
            .AsNoTracking()
            .FirstOrDefaultAsync(
                h => h.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<DecisionHistory>> GetByCreditDecisionIdAsync(
        Guid creditDecisionId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DecisionHistories
            .AsNoTracking()
            .Where(h => h.CreditDecisionId == creditDecisionId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        DecisionHistory history,
        CancellationToken cancellationToken = default)
    {
        await _context.DecisionHistories.AddAsync(
            history,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}