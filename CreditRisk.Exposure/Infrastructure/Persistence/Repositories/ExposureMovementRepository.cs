using Exposure.Domain.Entities;
using Exposure.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exposure.Infrastructure.Persistence.Repositories;

public class ExposureMovementRepository : IExposureMovementRepository
{
    private readonly CreditRiskDbContext _context;

    public ExposureMovementRepository(CreditRiskDbContext context)
    {
        _context = context;
    }

    public async Task<ExposureMovement?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ExposureMovements
            .AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ExposureMovement>> GetByCreditExposureIdAsync(
        Guid creditExposureId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ExposureMovements
            .AsNoTracking()
            .Where(m => m.CreditExposureId == creditExposureId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        ExposureMovement movement,
        CancellationToken cancellationToken = default)
    {
        await _context.ExposureMovements.AddAsync(
            movement,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}