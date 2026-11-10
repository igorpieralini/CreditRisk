using CreditRisk.RiskDecision.Domain.Enums;

namespace CreditRisk.RiskDecision.Domain.Entities;

public class DecisionHistory
{
    public Guid Id { get; private set; }
    public Guid CreditDecisionId { get; private set; }

    public DecisionStatus Status { get; private set; }
    public string Reason { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public CreditDecision CreditDecision { get; private set; } = null!;

    private DecisionHistory()
    {
    }

    public DecisionHistory(
        Guid creditDecisionId,
        DecisionStatus status,
        string reason)
    {
        if (creditDecisionId == Guid.Empty)
            throw new ArgumentException("A decisão de crédito é obrigatória.");

        if (!Enum.IsDefined(status))
            throw new ArgumentException("O status da decisão é inválido.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("A justificativa é obrigatória.");

        Id = Guid.NewGuid();
        CreditDecisionId = creditDecisionId;
        Status = status;
        Reason = reason.Trim();
        CreatedAt = DateTime.UtcNow;
    }
}