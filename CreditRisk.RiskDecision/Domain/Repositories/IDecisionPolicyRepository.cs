using CreditRisk.RiskDecision.Domain.Entities;

namespace CreditRisk.RiskDecision.Domain.Repositories;

public interface IDecisionPolicyRepository
{
    Task<DecisionPolicy?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DecisionPolicy?> GetActiveAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DecisionPolicy>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        DecisionPolicy policy,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        DecisionPolicy policy,
        CancellationToken cancellationToken = default);
}