namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeValuationRequest
{
    public Guid GuaranteeId { get; set; }
    public decimal MarketValue { get; set; }
    public decimal HaircutPercentage { get; set; }
}