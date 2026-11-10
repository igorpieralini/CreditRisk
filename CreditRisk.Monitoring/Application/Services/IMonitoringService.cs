using CreditRisk.Monitoring.Application.DTOs;

namespace CreditRisk.Monitoring.Application.Services;

public interface IMonitoringService
{
    Task<MonitoringResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<MonitoringResponse?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MonitoringResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<MonitoringResponse> CreateAsync(
        MonitoringRequest request,
        CancellationToken cancellationToken = default);

    Task<MonitoringResponse> UpdateAsync(
        Guid id,
        MonitoringRequest request,
        CancellationToken cancellationToken = default);

    Task RegisterEventAsync(
        MonitoringEventRequest request,
        CancellationToken cancellationToken = default);
}