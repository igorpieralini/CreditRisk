namespace CreditRisk.CreditAnalysis.Domain.Calculations;

public class InterestCalculator
{
    public decimal CalculateMonthlyRate(decimal annualInterestRate)
    {
        if (annualInterestRate < 0)
            throw new ArgumentException("A taxa de juros não pode ser negativa.");

        if (annualInterestRate == 0)
            return 0;

        double annualRate = (double)annualInterestRate / 100;

        double monthlyRate = Math.Pow(1 + annualRate, 1.0 / 12) - 1;

        return Math.Round((decimal)monthlyRate * 100, 6);
    }
}