namespace Exposure.Domain.Rules;

public class ExposureConcentrationRule
{
    public bool Validate(decimal concentrationPercentage, decimal maximumConcentration)
    {
        if (concentrationPercentage < 0 || concentrationPercentage > 100)
            throw new ArgumentException("A concentração deve estar entre 0 e 100%.");

        if (maximumConcentration <= 0 || maximumConcentration > 100)
            throw new ArgumentException("A concentração máxima deve estar entre 0 e 100%.");

        return concentrationPercentage <= maximumConcentration;
    }
}