using CreditRisk.RiskDecision.Domain.Entities;

namespace CreditRisk.RiskDecision.Domain.Repositories;

public interface ICreditDecisionRepository
{
    Task<CreditDecision?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditDecision>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditDecision>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditDecision decision,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CreditDecision decision,
        CancellationToken cancellationToken = default);
}