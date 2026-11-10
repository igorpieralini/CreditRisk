namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeAnalysisResponse
{
    public Guid GuaranteeId { get; set; }
    public Guid CreditOperationId { get; set; }
    public decimal OperationAmount { get; set; }
    public decimal MarketValue { get; set; }
    public decimal EligibleValue { get; set; }
    public decimal HaircutPercentage { get; set; }
    public decimal Ltv { get; set; }
    public decimal CoveragePercentage { get; set; }
    public bool IsSufficient { get; set; }
    public bool IsWithinMaximumLtv { get; set; }
}