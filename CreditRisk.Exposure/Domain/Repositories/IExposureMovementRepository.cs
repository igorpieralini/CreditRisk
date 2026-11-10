using Exposure.Domain.Entities;

namespace Exposure.Domain.Repositories;

public interface IExposureMovementRepository
{
    Task<ExposureMovement?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExposureMovement>> GetByCreditExposureIdAsync(
        Guid creditExposureId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        ExposureMovement movement,
        CancellationToken cancellationToken = default);
}