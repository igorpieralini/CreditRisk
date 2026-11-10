namespace CreditRisk.RiskDecision.Domain.Rules;

public class ExposureLimitRule
{
    public bool Validate(
        decimal limitUtilizationPercentage,
        decimal maximumLimitUtilization)
    {
        if (limitUtilizationPercentage < 0)
            throw new ArgumentException(
                "A utilização do limite não pode ser negativa.");

        if (maximumLimitUtilization <= 0 || maximumLimitUtilization > 100)
            throw new ArgumentException(
                "A utilização máxima permitida deve estar entre 0 e 100.");

        return limitUtilizationPercentage <= maximumLimitUtilization;
    }
}