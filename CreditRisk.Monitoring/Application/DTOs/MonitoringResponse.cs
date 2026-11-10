namespace CreditRisk.Monitoring.Application.DTOs;

public class MonitoringResponse
{
    public Guid Id { get; set; }

    public Guid CreditOperationId { get; set; }

    public decimal RiskScore { get; set; }

    public decimal GuaranteeCoveragePercentage { get; set; }

    public decimal LimitUtilizationPercentage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}