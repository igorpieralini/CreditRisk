using GuaranteeEntity = global::CreditRisk.Guarantee.Domain.Entities.Guarantee;

namespace CreditRisk.Guarantee.Domain.Repositories;

public interface IGuaranteeRepository
{
    Task<GuaranteeEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(GuaranteeEntity guarantee, CancellationToken cancellationToken = default);

    Task UpdateAsync(GuaranteeEntity guarantee, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}