namespace CreditRisk.Monitoring.Application.DTOs;

public class MonitoringRequest
{
    public Guid CreditOperationId { get; set; }

    public decimal RiskScore { get; set; }

    public decimal GuaranteeCoveragePercentage { get; set; }

    public decimal LimitUtilizationPercentage { get; set; }
}