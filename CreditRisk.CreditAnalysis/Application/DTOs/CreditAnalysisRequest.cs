using CreditRisk.CreditAnalysis.Domain.Enums;

namespace CreditRisk.CreditAnalysis.Application.DTOs;

public class CreditAnalysisRequest
{
    public decimal Amount { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int PaymentTermMonths { get; set; }
    public decimal AvailableLimit { get; set; }
    public int MaximumTermMonths { get; set; }
    public CreditType CreditType { get; set; }
}