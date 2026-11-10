namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeValuationResponse
{
    public Guid Id { get; set; }
    public Guid GuaranteeId { get; set; }
    public decimal MarketValue { get; set; }
    public decimal HaircutPercentage { get; set; }
    public decimal EligibleValue { get; set; }
    public DateTime ValuationDate { get; set; }
}