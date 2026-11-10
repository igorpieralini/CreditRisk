namespace CreditRisk.RiskDecision.Application.DTOs;

public class RiskDecisionRequest
{
    public Guid CreditOperationId { get; set; }

    public decimal RequestedAmount { get; set; }

    public decimal CreditScore { get; set; }

    public decimal GuaranteeCoveragePercentage { get; set; }

    public decimal LimitUtilizationPercentage { get; set; }
}