namespace CreditRisk.CreditAnalysis.Domain.Calculations;

public class InstallmentCalculator
{
    public decimal CalculateInstallment(
        decimal amount,
        decimal monthlyInterestRate,
        int paymentTermMonths)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        if (monthlyInterestRate < 0)
            throw new ArgumentException("A taxa mensal não pode ser negativa.");

        if (paymentTermMonths <= 0)
            throw new ArgumentException("O prazo deve ser maior que zero.");

        if (monthlyInterestRate == 0)
            return Math.Round(amount / paymentTermMonths, 2);

        double rate = (double)monthlyInterestRate / 100;
        double factor = Math.Pow(1 + rate, paymentTermMonths);

        double installment = (double)amount * (rate * factor) / (factor - 1);

        return Math.Round((decimal)installment, 2);
    }
}