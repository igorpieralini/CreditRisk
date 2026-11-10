using Exposure.Domain.Entities;

namespace Exposure.Domain.Repositories;

public interface ICreditExposureRepository
{
    Task<CreditExposure?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<CreditExposure?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditExposure>> GetByCreditLimitIdAsync(
        Guid creditLimitId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditExposure>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditExposure exposure,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CreditExposure exposure,
        CancellationToken cancellationToken = default);
}