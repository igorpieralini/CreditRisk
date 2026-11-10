using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Domain.Rules;

public class ExposureAlertRule
{
    public AlertLevel? Evaluate(decimal limitUtilizationPercentage)
    {
        if (limitUtilizationPercentage < 0)
            throw new ArgumentException(
                "A utilização do limite não pode ser negativa.");

        if (limitUtilizationPercentage >= 100)
            return AlertLevel.High;

        if (limitUtilizationPercentage >= 90)
            return AlertLevel.Medium;

        if (limitUtilizationPercentage >= 80)
            return AlertLevel.Low;

        return null;
    }
}