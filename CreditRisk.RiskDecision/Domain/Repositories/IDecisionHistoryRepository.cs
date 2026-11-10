using CreditRisk.RiskDecision.Domain.Entities;

namespace CreditRisk.RiskDecision.Domain.Repositories;

public interface IDecisionHistoryRepository
{
    Task<DecisionHistory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DecisionHistory>> GetByCreditDecisionIdAsync(
        Guid creditDecisionId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        DecisionHistory history,
        CancellationToken cancellationToken = default);
}