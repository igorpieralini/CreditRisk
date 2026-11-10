namespace CreditRisk.Guarantee.Domain.Rules;

public class GuaranteeSufficiencyRule
{
    public bool Validate(decimal eligibleValue, decimal operationAmount)
    {
        if (eligibleValue < 0)
            throw new ArgumentException("O valor elegível não pode ser negativo.");

        if (operationAmount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        return eligibleValue >= operationAmount;
    }
}