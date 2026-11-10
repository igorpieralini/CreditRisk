namespace CreditRisk.Guarantee.Domain.Calculations;

public class CoverageCalculator
{
    public decimal Calculate(decimal eligibleValue, decimal operationAmount)
    {
        if (eligibleValue < 0)
            throw new ArgumentException("O valor elegível não pode ser negativo.");

        if (operationAmount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        return Math.Round(
            eligibleValue / operationAmount * 100m,
            2
        );
    }
}