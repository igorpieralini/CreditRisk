namespace CreditRisk.CreditAnalysis.Domain.Models;

public class CreditAnalysisResult
{
    public Guid OperationId { get; private set; }
    public decimal MonthlyInterestRate { get; private set; }
    public decimal InstallmentAmount { get; private set; }
    public decimal TotalPayment { get; private set; }
    public decimal TotalInterest { get; private set; }

    public CreditAnalysisResult(
        Guid operationId,
        decimal monthlyInterestRate,
        decimal installmentAmount,
        decimal totalPayment,
        decimal totalInterest)
    {
        OperationId = operationId;
        MonthlyInterestRate = monthlyInterestRate;
        InstallmentAmount = installmentAmount;
        TotalPayment = totalPayment;
        TotalInterest = totalInterest;
    }
}