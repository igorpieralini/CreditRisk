namespace Exposure.Application.DTOs;

public class ExposureAnalysisRequest
{
    public Guid CreditLimitId { get; set; }
    public decimal AdditionalAmount { get; set; }
    public decimal CategoryExposure { get; set; }
    public decimal TotalPortfolioExposure { get; set; }
    public decimal MaximumConcentrationPercentage { get; set; }
}