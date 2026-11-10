
using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Domain.Entities;

public class RiskAlert
{
    public Guid Id { get; private set; }
    public Guid CreditMonitoringId { get; private set; }

    public AlertLevel Level { get; private set; }
    public AlertStatus Status { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }

    public CreditMonitoring CreditMonitoring { get; private set; } = null!;

    private RiskAlert()
    {
    }

    public RiskAlert(
        Guid creditMonitoringId,
        AlertLevel level,
        string title,
        string description)
    {
        if (creditMonitoringId == Guid.Empty)
            throw new ArgumentException(
                "O monitoramento de crédito é obrigatório.");

        if (!Enum.IsDefined(level))
            throw new ArgumentException(
                "O nível do alerta é inválido.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "O título do alerta é obrigatório.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "A descrição do alerta é obrigatória.");

        Id = Guid.NewGuid();
        CreditMonitoringId = creditMonitoringId;

        Level = level;
        Status = AlertStatus.Open;

        Title = title.Trim();
        Description = description.Trim();

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Acknowledge()
    {
        if (Status != AlertStatus.Open)
            throw new InvalidOperationException(
                "Somente alertas abertos podem ser reconhecidos.");

        Status = AlertStatus.Acknowledged;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve()
    {
        if (Status == AlertStatus.Resolved)
            throw new InvalidOperationException(
                "O alerta já está resolvido.");

        Status = AlertStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
        UpdatedAt = ResolvedAt.Value;
    }
}