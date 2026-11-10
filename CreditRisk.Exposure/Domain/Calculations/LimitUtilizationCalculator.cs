namespace Exposure.Domain.Calculations;

public class LimitUtilizationCalculator
{
    public decimal Calculate(decimal totalExposure, decimal totalLimit)
    {
        if (totalExposure < 0)
            throw new ArgumentException("A exposição total não pode ser negativa.");

        if (totalLimit <= 0)
            throw new ArgumentException("O limite total deve ser maior que zero.");

        return Math.Round(
            totalExposure / totalLimit * 100m,
            2
        );
    }
}