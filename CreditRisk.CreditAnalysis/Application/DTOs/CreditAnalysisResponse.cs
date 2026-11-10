namespace CreditRisk.CreditAnalysis.Application.DTOs;

public class CreditAnalysisResponse
{
    public Guid OperationId { get; set; }
    public decimal MonthlyInterestRate { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal TotalInterest { get; set; }
    public bool IsWithinCreditLimit { get; set; }
    public bool IsWithinPaymentTerm { get; set; }
}