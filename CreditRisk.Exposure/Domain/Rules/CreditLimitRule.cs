namespace Exposure.Domain.Rules;

public class CreditLimitRule
{
    public bool Validate(decimal totalExposure, decimal totalLimit)
    {
        if (totalExposure < 0)
            throw new ArgumentException("A exposição total não pode ser negativa.");

        if (totalLimit <= 0)
            throw new ArgumentException("O limite de crédito deve ser maior que zero.");

        return totalExposure <= totalLimit;
    }

    public bool CanIncrease(
        decimal currentExposure,
        decimal additionalAmount,
        decimal totalLimit)
    {
        if (currentExposure < 0)
            throw new ArgumentException("A exposição atual não pode ser negativa.");

        if (additionalAmount <= 0)
            throw new ArgumentException("O valor adicional deve ser maior que zero.");

        if (totalLimit <= 0)
            throw new ArgumentException("O limite de crédito deve ser maior que zero.");

        return currentExposure + additionalAmount <= totalLimit;
    }
}