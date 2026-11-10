using CreditRisk.Monitoring.Domain.Entities;

namespace CreditRisk.Monitoring.Domain.Repositories;

public interface IMonitoringEventRepository
{
    Task<MonitoringEvent?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MonitoringEvent>> GetByCreditMonitoringIdAsync(
        Guid creditMonitoringId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MonitoringEvent>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        MonitoringEvent monitoringEvent,
        CancellationToken cancellationToken = default);
}