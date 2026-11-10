
namespace CreditRisk.Monitoring.Domain.Entities;

public class CreditMonitoring
{
    public Guid Id { get; private set; }
    public Guid CreditOperationId { get; private set; }

    public decimal RiskScore { get; private set; }
    public decimal GuaranteeCoveragePercentage { get; private set; }
    public decimal LimitUtilizationPercentage { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<RiskAlert> Alerts { get; private set; }
        = new List<RiskAlert>();

    public ICollection<MonitoringEvent> Events { get; private set; }
        = new List<MonitoringEvent>();

    private CreditMonitoring()
    {
    }

    public CreditMonitoring(
        Guid creditOperationId,
        decimal riskScore,
        decimal guaranteeCoveragePercentage,
        decimal limitUtilizationPercentage)
    {
        if (creditOperationId == Guid.Empty)
            throw new ArgumentException(
                "A operação de crédito é obrigatória.");

        ValidateIndicators(
            riskScore,
            guaranteeCoveragePercentage,
            limitUtilizationPercentage);

        Id = Guid.NewGuid();
        CreditOperationId = creditOperationId;

        RiskScore = riskScore;
        GuaranteeCoveragePercentage = guaranteeCoveragePercentage;
        LimitUtilizationPercentage = limitUtilizationPercentage;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateIndicators(
        decimal riskScore,
        decimal guaranteeCoveragePercentage,
        decimal limitUtilizationPercentage)
    {
        ValidateIndicators(
            riskScore,
            guaranteeCoveragePercentage,
            limitUtilizationPercentage);

        RiskScore = riskScore;
        GuaranteeCoveragePercentage = guaranteeCoveragePercentage;
        LimitUtilizationPercentage = limitUtilizationPercentage;

        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateIndicators(
        decimal riskScore,
        decimal guaranteeCoveragePercentage,
        decimal limitUtilizationPercentage)
    {
        if (riskScore < 0 || riskScore > 100)
            throw new ArgumentException(
                "O score de risco deve estar entre 0 e 100.");

        if (guaranteeCoveragePercentage < 0)
            throw new ArgumentException(
                "A cobertura de garantias não pode ser negativa.");

        if (limitUtilizationPercentage < 0)
            throw new ArgumentException(
                "A utilização do limite não pode ser negativa.");
    }
}