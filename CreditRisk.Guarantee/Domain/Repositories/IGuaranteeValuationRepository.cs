using CreditRisk.Guarantee.Domain.Entities;

namespace CreditRisk.Guarantee.Domain.Repositories;

public interface IGuaranteeValuationRepository
{
    Task<GuaranteeValuation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeValuation>> GetByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default);

    Task<GuaranteeValuation?> GetLatestByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        GuaranteeValuation valuation,
        CancellationToken cancellationToken = default);
}