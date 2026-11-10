
namespace CreditRisk.RiskDecision.Domain.Entities;

public class DecisionPolicy
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public decimal MinimumRiskScore { get; private set; }
    public decimal MinimumGuaranteeCoverage { get; private set; }
    public decimal MaximumLimitUtilization { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private DecisionPolicy()
    {
    }

    public DecisionPolicy(
        string name,
        decimal minimumRiskScore,
        decimal minimumGuaranteeCoverage,
        decimal maximumLimitUtilization)
    {
        Validate(
            name,
            minimumRiskScore,
            minimumGuaranteeCoverage,
            maximumLimitUtilization);

        Id = Guid.NewGuid();
        Name = name.Trim();
        MinimumRiskScore = minimumRiskScore;
        MinimumGuaranteeCoverage = minimumGuaranteeCoverage;
        MaximumLimitUtilization = maximumLimitUtilization;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Update(
        string name,
        decimal minimumRiskScore,
        decimal minimumGuaranteeCoverage,
        decimal maximumLimitUtilization)
    {
        Validate(
            name,
            minimumRiskScore,
            minimumGuaranteeCoverage,
            maximumLimitUtilization);

        Name = name.Trim();
        MinimumRiskScore = minimumRiskScore;
        MinimumGuaranteeCoverage = minimumGuaranteeCoverage;
        MaximumLimitUtilization = maximumLimitUtilization;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException(
                "A política já está ativa.");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException(
                "A política já está inativa.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(
        string name,
        decimal minimumRiskScore,
        decimal minimumGuaranteeCoverage,
        decimal maximumLimitUtilization)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "O nome da política é obrigatório.");

        if (name.Trim().Length > 180)
            throw new ArgumentException(
                "O nome não pode ultrapassar 180 caracteres.");

        if (minimumRiskScore < 0 || minimumRiskScore > 100)
            throw new ArgumentException(
                "O score mínimo deve estar entre 0 e 100.");

        if (minimumGuaranteeCoverage < 0)
            throw new ArgumentException(
                "A cobertura mínima de garantias não pode ser negativa.");

        if (maximumLimitUtilization <= 0 ||
            maximumLimitUtilization > 100)
            throw new ArgumentException(
                "A utilização máxima deve estar entre 0 e 100.");

    }
}