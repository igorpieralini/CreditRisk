using CreditRisk.RiskDecision.Application.DTOs;
using CreditRisk.RiskDecision.Domain.Calculations;
using CreditRisk.RiskDecision.Domain.Entities;
using CreditRisk.RiskDecision.Domain.Rules;

namespace CreditRisk.RiskDecision.Application.Services;

public class RiskEvaluationService : IRiskEvaluationService
{
    private readonly RiskScoreCalculator _scoreCalculator;
    private readonly CreditScoreRule _creditScoreRule;
    private readonly GuaranteeCoverageRule _guaranteeCoverageRule;
    private readonly ExposureLimitRule _exposureLimitRule;

    public RiskEvaluationService(
        RiskScoreCalculator scoreCalculator,
        CreditScoreRule creditScoreRule,
        GuaranteeCoverageRule guaranteeCoverageRule,
        ExposureLimitRule exposureLimitRule)
    {
        _scoreCalculator = scoreCalculator;
        _creditScoreRule = creditScoreRule;
        _guaranteeCoverageRule = guaranteeCoverageRule;
        _exposureLimitRule = exposureLimitRule;
    }

    public Task<RiskIndicators> EvaluateAsync(
        RiskDecisionRequest request,
        DecisionPolicy policy,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(policy);

        cancellationToken.ThrowIfCancellationRequested();

        if (!policy.IsActive)
            throw new InvalidOperationException(
                "A política de decisão está inativa.");

        decimal riskScore = _scoreCalculator.Calculate(
            request.CreditScore,
            request.GuaranteeCoveragePercentage,
            request.LimitUtilizationPercentage
        );

        bool meetsCreditScore = _creditScoreRule.Validate(
            riskScore,
            policy.MinimumRiskScore
        );

        bool meetsGuaranteeCoverage = _guaranteeCoverageRule.Validate(
            request.GuaranteeCoveragePercentage,
            policy.MinimumGuaranteeCoverage
        );

        bool meetsExposureLimit = _exposureLimitRule.Validate(
            request.LimitUtilizationPercentage,
            policy.MaximumLimitUtilization
        );

        var indicators = new RiskIndicators
        {
            CreditScore = request.CreditScore,
            RiskScore = riskScore,
            GuaranteeCoveragePercentage = request.GuaranteeCoveragePercentage,
            LimitUtilizationPercentage = request.LimitUtilizationPercentage,
            MeetsCreditScoreRequirement = meetsCreditScore,
            MeetsGuaranteeCoverageRequirement = meetsGuaranteeCoverage,
            MeetsExposureLimitRequirement = meetsExposureLimit
        };

        return Task.FromResult(indicators);
    }
}
