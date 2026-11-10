namespace CreditRisk.CreditAnalysis.Domain.Rules;

public class CreditLimitRule
{
    public bool Validate(decimal amount, decimal availableLimit)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        if (availableLimit < 0)
            throw new ArgumentException("O limite disponível não pode ser negativo.");

        return amount <= availableLimit;
    }
}