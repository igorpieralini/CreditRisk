namespace CreditRisk.CreditAnalysis.Domain.Models;

public class CreditOperation
{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public decimal AnnualInterestRate { get; private set; }
    public int PaymentTermMonths { get; private set; }

    public CreditOperation(
        decimal amount,
        decimal annualInterestRate,
        int paymentTermMonths)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        if (annualInterestRate < 0)
            throw new ArgumentException("A taxa de juros não pode ser negativa.");

        if (paymentTermMonths <= 0)
            throw new ArgumentException("O prazo deve ser maior que zero.");

        Id = Guid.NewGuid();
        Amount = amount;
        AnnualInterestRate = annualInterestRate;
        PaymentTermMonths = paymentTermMonths;
    }
}