using CreditRisk.RiskDecision.Application.DTOs;

namespace CreditRisk.RiskDecision.Application.Services;

public interface IRiskDecisionService
{
    Task<RiskDecisionResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RiskDecisionResponse>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RiskDecisionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DecisionHistoryResponse>> GetHistoryAsync(
        Guid creditDecisionId,
        CancellationToken cancellationToken = default);

    Task<RiskDecisionResponse> EvaluateAsync(
        RiskDecisionRequest request,
        CancellationToken cancellationToken = default);
}