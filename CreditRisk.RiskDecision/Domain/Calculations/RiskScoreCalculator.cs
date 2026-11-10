namespace CreditRisk.RiskDecision.Domain.Calculations;

public class RiskScoreCalculator
{
    public decimal Calculate(
        decimal creditScore,
        decimal guaranteeCoveragePercentage,
        decimal limitUtilizationPercentage)
    {
        if (creditScore < 0 || creditScore > 100)
            throw new ArgumentException(
                "O score de crédito deve estar entre 0 e 100.");

        if (guaranteeCoveragePercentage < 0)
            throw new ArgumentException(
                "A cobertura de garantias não pode ser negativa.");

        if (limitUtilizationPercentage < 0)
            throw new ArgumentException(
                "A utilização do limite não pode ser negativa.");

        decimal normalizedGuarantee = Math.Min(
            guaranteeCoveragePercentage, 100m);

        decimal normalizedUtilization = Math.Min(
            limitUtilizationPercentage, 100m);

        decimal score =
            (creditScore * 0.50m) +
            (normalizedGuarantee * 0.30m) +
            ((100m - normalizedUtilization) * 0.20m);

        return Math.Round(score, 2);
    }
}