using Exposure.Domain.Entities;

namespace Exposure.Domain.Repositories;

public interface ICreditLimitRepository
{
    Task<CreditLimit?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditLimit>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditLimit creditLimit,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CreditLimit creditLimit,
        CancellationToken cancellationToken = default);
}