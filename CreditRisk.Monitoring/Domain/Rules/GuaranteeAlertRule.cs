using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Domain.Rules;

public class GuaranteeAlertRule
{
    public AlertLevel? Evaluate(decimal guaranteeCoveragePercentage)
    {
        if (guaranteeCoveragePercentage < 0)
            throw new ArgumentException(
                "A cobertura de garantias não pode ser negativa.");

        if (guaranteeCoveragePercentage < 50)
            return AlertLevel.High;

        if (guaranteeCoveragePercentage < 80)
            return AlertLevel.Medium;

        if (guaranteeCoveragePercentage < 100)
            return AlertLevel.Low;

        return null;
    }
}