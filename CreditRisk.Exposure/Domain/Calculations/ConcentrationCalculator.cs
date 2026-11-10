namespace Exposure.Domain.Calculations;

public class ConcentrationCalculator
{
    public decimal Calculate(decimal categoryExposure, decimal totalExposure)
    {
        if (categoryExposure < 0)
            throw new ArgumentException("A exposição da categoria não pode ser negativa.");

        if (totalExposure < 0)
            throw new ArgumentException("A exposição total não pode ser negativa.");

        if (categoryExposure > totalExposure)
            throw new ArgumentException(
                "A exposição da categoria não pode ultrapassar a exposição total.");

        if (totalExposure == 0)
            return 0;

        return Math.Round(
            categoryExposure / totalExposure * 100m,
            2
        );
    }
}