using CreditRisk.Guarantee.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using GuaranteeEntity = global::CreditRisk.Guarantee.Domain.Entities.Guarantee;

namespace CreditRisk.Guarantee.Infrastructure.Persistence.Repositories;

public class GuaranteeRepository : IGuaranteeRepository
{
    private readonly CreditRiskDbContext _context;

    public GuaranteeRepository(CreditRiskDbContext context)
    {
        _context = context;
    }

    public async Task<GuaranteeEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Guarantees
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GuaranteeEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Guarantees
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        GuaranteeEntity guarantee,
        CancellationToken cancellationToken = default)
    {
        await _context.Guarantees.AddAsync(guarantee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        GuaranteeEntity guarantee,
        CancellationToken cancellationToken = default)
    {
        _context.Guarantees.Update(guarantee);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var guarantee = await _context.Guarantees
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (guarantee is null)
            throw new KeyNotFoundException("Garantia não encontrada.");

        _context.Guarantees.Remove(guarantee);
        await _context.SaveChangesAsync(cancellationToken);
    }
}