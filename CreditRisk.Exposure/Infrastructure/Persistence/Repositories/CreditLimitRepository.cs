using Exposure.Domain.Entities;
using Exposure.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exposure.Infrastructure.Persistence.Repositories;

public class CreditLimitRepository : ICreditLimitRepository
{
    private readonly CreditRiskDbContext _context;

    public CreditLimitRepository(CreditRiskDbContext context)
    {
        _context = context;
    }

    public async Task<CreditLimit?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditLimits
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<CreditLimit>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditLimits
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CreditLimit creditLimit,
        CancellationToken cancellationToken = default)
    {
        await _context.CreditLimits.AddAsync(
            creditLimit,
            cancellationToken
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        CreditLimit creditLimit,
        CancellationToken cancellationToken = default)
    {
        _context.CreditLimits.Update(creditLimit);

        await _context.SaveChangesAsync(cancellationToken);
    }
}