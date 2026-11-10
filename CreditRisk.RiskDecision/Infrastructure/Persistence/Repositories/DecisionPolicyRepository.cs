using CreditRisk.RiskDecision.Domain.Entities;
using CreditRisk.RiskDecision.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.RiskDecision.Infrastructure.Persistence.Repositories;

public class DecisionPolicyRepository : IDecisionPolicyRepository
{
    private readonly RiskDecisionDbContext _context;

    public DecisionPolicyRepository(RiskDecisionDbContext context)
    {
        _context = context;
    }

    public async Task<DecisionPolicy?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.DecisionPolicies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    public async Task<DecisionPolicy?> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.DecisionPolicies
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DecisionPolicy>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.DecisionPolicies
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        DecisionPolicy policy,
        CancellationToken cancellationToken = default)
    {
        await _context.DecisionPolicies.AddAsync(
            policy,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        DecisionPolicy policy,
        CancellationToken cancellationToken = default)
    {
        _context.DecisionPolicies.Update(policy);

        await _context.SaveChangesAsync(cancellationToken);
    }
}