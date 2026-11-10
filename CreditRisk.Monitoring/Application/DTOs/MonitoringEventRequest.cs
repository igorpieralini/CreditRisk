using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Application.DTOs;

public class MonitoringEventRequest
{
    public Guid CreditOperationId { get; set; }

    public MonitoringEventType Type { get; set; }

    public string Description { get; set; } = string.Empty;
}