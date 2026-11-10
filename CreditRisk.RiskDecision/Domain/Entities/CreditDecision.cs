
using CreditRisk.RiskDecision.Domain.Enums;

namespace CreditRisk.RiskDecision.Domain.Entities;

public class CreditDecision
{
    public Guid Id { get; private set; }
    public Guid CreditOperationId { get; private set; }

    public decimal RequestedAmount { get; private set; }
    public decimal RiskScore { get; private set; }
    public decimal GuaranteeCoveragePercentage { get; private set; }
    public decimal LimitUtilizationPercentage { get; private set; }

    public DecisionStatus Status { get; private set; }
    public string Reason { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public ICollection<DecisionHistory> Histories { get; private set; }
        = new List<DecisionHistory>();

    private CreditDecision()
    {
    }

    public CreditDecision(
        Guid creditOperationId,
        decimal requestedAmount,
        decimal riskScore,
        decimal guaranteeCoveragePercentage,
        decimal limitUtilizationPercentage,
        DecisionStatus status,
        string reason)
    {
        if (creditOperationId == Guid.Empty)
            throw new ArgumentException(
                "A operação de crédito é obrigatória.");

        if (requestedAmount <= 0)
            throw new ArgumentException(
                "O valor solicitado deve ser maior que zero.");

        if (riskScore < 0 || riskScore > 100)
            throw new ArgumentException(
                "O score de risco deve estar entre 0 e 100.");

        if (guaranteeCoveragePercentage < 0)
            throw new ArgumentException(
                "A cobertura de garantias não pode ser negativa.");

        if (limitUtilizationPercentage < 0)
            throw new ArgumentException(
                "A utilização do limite não pode ser negativa.");

        if (!Enum.IsDefined(status))
            throw new ArgumentException(
                "O status da decisão é inválido.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException(
                "A justificativa da decisão é obrigatória.");

        Id = Guid.NewGuid();
        CreditOperationId = creditOperationId;
        RequestedAmount = requestedAmount;
        RiskScore = riskScore;
        GuaranteeCoveragePercentage = guaranteeCoveragePercentage;
        LimitUtilizationPercentage = limitUtilizationPercentage;
        Status = status;
        Reason = reason.Trim();
        CreatedAt = DateTime.UtcNow;
    }
}