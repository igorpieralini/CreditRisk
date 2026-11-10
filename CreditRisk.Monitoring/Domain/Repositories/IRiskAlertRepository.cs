using CreditRisk.Monitoring.Domain.Entities;

namespace CreditRisk.Monitoring.Domain.Repositories;

public interface IRiskAlertRepository
{
    Task<RiskAlert?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RiskAlert>> GetByCreditMonitoringIdAsync(
        Guid creditMonitoringId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RiskAlert>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        RiskAlert alert,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        RiskAlert alert,
        CancellationToken cancellationToken = default);
}