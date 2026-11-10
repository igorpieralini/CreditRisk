using CreditRisk.Monitoring.Application.DTOs;

namespace CreditRisk.Monitoring.Application.Services;

public interface IAlertService
{
    Task<AlertResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlertResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlertResponse>> GetByCreditMonitoringIdAsync(
        Guid creditMonitoringId,
        CancellationToken cancellationToken = default);

    Task<AlertResponse> AcknowledgeAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<AlertResponse> ResolveAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}