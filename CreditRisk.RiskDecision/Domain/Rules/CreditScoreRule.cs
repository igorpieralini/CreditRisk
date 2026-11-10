namespace CreditRisk.RiskDecision.Domain.Rules;

public class CreditScoreRule
{
    public bool Validate(
        decimal riskScore,
        decimal minimumRiskScore)
    {
        if (riskScore < 0 || riskScore > 100)
            throw new ArgumentException(
                "O score de risco deve estar entre 0 e 100.");

        if (minimumRiskScore < 0 || minimumRiskScore > 100)
            throw new ArgumentException(
                "O score mínimo deve estar entre 0 e 100.");

        return riskScore >= minimumRiskScore;
    }
}