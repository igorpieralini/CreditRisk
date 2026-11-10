namespace Exposure.Application.DTOs;

public class ExposureAnalysisResponse
{
    public Guid CreditLimitId { get; set; }
    public decimal TotalLimit { get; set; }
    public decimal CurrentExposure { get; set; }
    public decimal AdditionalAmount { get; set; }
    public decimal ProjectedExposure { get; set; }
    public decimal AvailableLimit { get; set; }
    public decimal UtilizationPercentage { get; set; }
    public decimal ConcentrationPercentage { get; set; }
    public bool IsWithinCreditLimit { get; set; }
    public bool IsWithinConcentrationLimit { get; set; }
}