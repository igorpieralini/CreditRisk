namespace CreditRisk.Guarantee.Domain.Rules;

public class MaximumLtvRule
{
    public bool Validate(decimal ltv, decimal maximumLtv)
    {
        if (ltv < 0)
            throw new ArgumentException("O LTV não pode ser negativo.");

        if (maximumLtv <= 0)
            throw new ArgumentException("O LTV máximo deve ser maior que zero.");

        return ltv <= maximumLtv;
    }
}