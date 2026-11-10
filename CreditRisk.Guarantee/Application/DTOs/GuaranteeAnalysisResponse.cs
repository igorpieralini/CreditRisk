namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeAnalysisRequest
{
    public Guid GuaranteeId { get; set; }
    public Guid CreditOperationId { get; set; }
    public decimal OperationAmount { get; set; }
    public decimal MarketValue { get; set; }
    public decimal HaircutPercentage { get; set; }
    public decimal MaximumLtv { get; set; }
}