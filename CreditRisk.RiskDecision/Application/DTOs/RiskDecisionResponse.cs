using CreditRisk.RiskDecision.Domain.Enums;

namespace CreditRisk.RiskDecision.Application.DTOs;

public class RiskDecisionResponse
{
    public Guid Id { get; set; }

    public Guid CreditOperationId { get; set; }

    public decimal RequestedAmount { get; set; }

    public decimal RiskScore { get; set; }

    public decimal GuaranteeCoveragePercentage { get; set; }

    public decimal LimitUtilizationPercentage { get; set; }

    public DecisionStatus Status { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}