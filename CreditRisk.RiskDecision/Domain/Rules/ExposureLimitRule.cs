namespace CreditRisk.RiskDecision.Domain.Rules;

public class GuaranteeCoverageRule
{
    public bool Validate(
        decimal guaranteeCoveragePercentage,
        decimal minimumGuaranteeCoverage)
    {
        if (guaranteeCoveragePercentage < 0)
            throw new ArgumentException(
                "A cobertura de garantias não pode ser negativa.");

        if (minimumGuaranteeCoverage < 0)
            throw new ArgumentException(
                "A cobertura mínima exigida não pode ser negativa.");

        return guaranteeCoveragePercentage >= minimumGuaranteeCoverage;
    }
}