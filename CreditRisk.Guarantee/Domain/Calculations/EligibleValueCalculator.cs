namespace CreditRisk.Guarantee.Domain.Calculations;

public class EligibleValueCalculator
{
    public decimal Calculate(decimal marketValue, decimal haircutPercentage)
    {
        if (marketValue <= 0)
            throw new ArgumentException("O valor de mercado deve ser maior que zero.");

        if (haircutPercentage < 0 || haircutPercentage > 100)
            throw new ArgumentException("O haircut deve estar entre 0 e 100%.");

        return Math.Round(
            marketValue * (1 - haircutPercentage / 100m),
            2
        );
    }
}