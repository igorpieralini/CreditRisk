using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Domain.Rules;

public class RiskScoreAlertRule
{
    public AlertLevel? Evaluate(decimal riskScore)
    {
        if (riskScore < 0 || riskScore > 100)
            throw new ArgumentException(
                "O score de risco deve estar entre 0 e 100.");

        if (riskScore < 40)
            return AlertLevel.High;

        if (riskScore < 60)
            return AlertLevel.Medium;

        if (riskScore < 75)
            return AlertLevel.Low;

        return null;
    }
}