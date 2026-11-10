namespace CreditRisk.Guarantee.Domain.Calculations;

public class LtvCalculator
{
    public decimal Calculate(decimal operationAmount, decimal guaranteeValue)
    {
        if (operationAmount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        if (guaranteeValue <= 0)
            throw new ArgumentException("O valor da garantia deve ser maior que zero.");

        return Math.Round(
            operationAmount / guaranteeValue * 100m,
            2
        );
    }
}