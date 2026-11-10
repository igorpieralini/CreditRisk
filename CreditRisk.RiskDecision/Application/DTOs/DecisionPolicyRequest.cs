namespace CreditRisk.RiskDecision.Application.DTOs;

public class DecisionPolicyRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal MinimumRiskScore { get; set; }

    public decimal MinimumGuaranteeCoverage { get; set; }

    public decimal MaximumLimitUtilization { get; set; }
}