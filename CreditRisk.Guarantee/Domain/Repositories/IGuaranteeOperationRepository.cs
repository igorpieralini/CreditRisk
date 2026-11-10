using CreditRisk.Guarantee.Domain.Entities;

namespace CreditRisk.Guarantee.Domain.Repositories;

public interface IGuaranteeOperationRepository
{
    Task<GuaranteeOperation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeOperation>> GetByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeOperation>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        GuaranteeOperation operation,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        GuaranteeOperation operation,
        CancellationToken cancellationToken = default);
}