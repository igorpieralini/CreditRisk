using CreditRisk.Monitoring.Domain.Entities;

namespace CreditRisk.Monitoring.Domain.Repositories;

public interface ICreditMonitoringRepository
{
    Task<CreditMonitoring?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<CreditMonitoring?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditMonitoring>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditMonitoring monitoring,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CreditMonitoring monitoring,
        CancellationToken cancellationToken = default);
}