namespace Exposure.Domain.Calculations;

public class ExposureCalculator
{
    public decimal CalculateTotalExposure(IEnumerable<decimal> outstandingAmounts)
    {
        ArgumentNullException.ThrowIfNull(outstandingAmounts);

        if (outstandingAmounts.Any(amount => amount < 0))
            throw new ArgumentException("Os valores de exposição não podem ser negativos.");

        return outstandingAmounts.Sum();
    }

    public decimal CalculateAvailableLimit(
        decimal totalLimit,
        decimal totalExposure)
    {
        if (totalLimit <= 0)
            throw new ArgumentException("O limite total deve ser maior que zero.");

        if (totalExposure < 0)
            throw new ArgumentException("A exposição total não pode ser negativa.");

        return Math.Max(0m, totalLimit - totalExposure);
    }
}