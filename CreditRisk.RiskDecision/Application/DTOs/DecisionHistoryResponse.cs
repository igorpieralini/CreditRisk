using CreditRisk.RiskDecision.Domain.Enums;

namespace CreditRisk.RiskDecision.Application.DTOs;

public class DecisionHistoryResponse
{
    public Guid Id { get; set; }

    public Guid CreditDecisionId { get; set; }

    public DecisionStatus Status { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}