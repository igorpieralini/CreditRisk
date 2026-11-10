using CreditRisk.RiskDecision.Application.DTOs;
using CreditRisk.RiskDecision.Domain.Entities;

namespace CreditRisk.RiskDecision.Application.Services;

public interface IRiskEvaluationService
{
    Task<RiskIndicators> EvaluateAsync(
        RiskDecisionRequest request,
        DecisionPolicy policy,
        CancellationToken cancellationToken = default);
}