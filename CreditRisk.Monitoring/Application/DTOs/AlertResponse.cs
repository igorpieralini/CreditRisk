using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Application.DTOs;

public class AlertResponse
{
    public Guid Id { get; set; }

    public Guid CreditMonitoringId { get; set; }

    public AlertLevel Level { get; set; }

    public AlertStatus Status { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }
}