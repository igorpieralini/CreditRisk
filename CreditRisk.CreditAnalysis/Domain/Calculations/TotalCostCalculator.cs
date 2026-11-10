namespace CreditRisk.CreditAnalysis.Domain.Calculations;

public class TotalCostCalculator
{
    public decimal CalculateTotalPayment(
        decimal installmentAmount,
        int paymentTermMonths)
    {
        if (installmentAmount <= 0)
            throw new ArgumentException("O valor da parcela deve ser maior que zero.");

        if (paymentTermMonths <= 0)
            throw new ArgumentException("O prazo deve ser maior que zero.");

        return Math.Round(installmentAmount * paymentTermMonths, 2);
    }

    public decimal CalculateTotalInterest(
        decimal totalPayment,
        decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.");

        if (totalPayment < amount)
            throw new ArgumentException("O pagamento total não pode ser inferior ao valor da operação.");

        return Math.Round(totalPayment - amount, 2);
    }
}