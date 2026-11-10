using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Domain.Entities;

public class MonitoringEvent
{
    public Guid Id { get; private set; }
    public Guid CreditMonitoringId { get; private set; }

    public MonitoringEventType Type { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public CreditMonitoring CreditMonitoring { get; private set; } = null!;

    private MonitoringEvent()
    {
    }

    public MonitoringEvent(
        Guid creditMonitoringId,
        MonitoringEventType type,
        string description)
    {
        if (creditMonitoringId == Guid.Empty)
            throw new ArgumentException(
                "O monitoramento de crédito é obrigatório.");

        if (!Enum.IsDefined(type))
            throw new ArgumentException(
                "O tipo de evento é inválido.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "A descrição do evento é obrigatória.");

        Id = Guid.NewGuid();
        CreditMonitoringId = creditMonitoringId;

        Type = type;
        Description = description.Trim();

        CreatedAt = DateTime.UtcNow;
    }
}