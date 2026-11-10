namespace CreditRisk.RiskDecision.Application.DTOs;

public class RiskIndicators
{
    public decimal CreditScore { get; set; }

    public decimal RiskScore { get; set; }

    public decimal GuaranteeCoveragePercentage { get; set; }

    public decimal LimitUtilizationPercentage { get; set; }

    public bool MeetsCreditScoreRequirement { get; set; }

    public bool MeetsGuaranteeCoverageRequirement { get; set; }

    public bool MeetsExposureLimitRequirement { get; set; }
}