
using CreditRisk.Guarantee.Domain.Entities;
using CreditRisk.Guarantee.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Guarantee.Infrastructure.Persistence.Repositories;

public class GuaranteeOperationRepository : IGuaranteeOperationRepository
{
    private readonly CreditRiskDbContext _context;

    public GuaranteeOperationRepository(CreditRiskDbContext context)
    {
        _context = context;
    }

    public async Task<GuaranteeOperation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.GuaranteeOperations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GuaranteeOperation>> GetByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.GuaranteeOperations
            .AsNoTracking()
            .Where(o => o.GuaranteeId == guaranteeId)
            .OrderByDescending(o => o.LinkedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<GuaranteeOperation>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.GuaranteeOperations
            .AsNoTracking()
            .Where(o => o.CreditOperationId == creditOperationId)
            .OrderByDescending(o => o.LinkedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        GuaranteeOperation operation,
        CancellationToken cancellationToken = default)
    {
        await _context.GuaranteeOperations.AddAsync(
            operation,
            cancellationToken
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        GuaranteeOperation operation,
        CancellationToken cancellationToken = default)
    {
        _context.GuaranteeOperations.Update(operation);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
