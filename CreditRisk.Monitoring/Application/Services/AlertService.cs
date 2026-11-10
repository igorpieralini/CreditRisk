
using CreditRisk.Monitoring.Application.DTOs;
using CreditRisk.Monitoring.Domain.Entities;
using CreditRisk.Monitoring.Domain.Repositories;

namespace CreditRisk.Monitoring.Application.Services;

public class AlertService : IAlertService
{
    private readonly IRiskAlertRepository _alertRepository;

    public AlertService(IRiskAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public async Task<AlertResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var alert = await _alertRepository.GetByIdAsync(
            id, cancellationToken);

        return alert is null ? null : MapToResponse(alert);
    }

    public async Task<IReadOnlyList<AlertResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var alerts = await _alertRepository.GetAllAsync(
            cancellationToken);

        return alerts.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<AlertResponse>> GetByCreditMonitoringIdAsync(
        Guid creditMonitoringId,
        CancellationToken cancellationToken = default)
    {
        var alerts = await _alertRepository.GetByCreditMonitoringIdAsync(
            creditMonitoringId, cancellationToken);

        return alerts.Select(MapToResponse).ToList();
    }

    public async Task<AlertResponse> AcknowledgeAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var alert = await GetAlertAsync(id, cancellationToken);

        alert.Acknowledge();

        await _alertRepository.UpdateAsync(
            alert, cancellationToken);

        return MapToResponse(alert);
    }

    public async Task<AlertResponse> ResolveAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var alert = await GetAlertAsync(id, cancellationToken);

        alert.Resolve();

        await _alertRepository.UpdateAsync(
            alert, cancellationToken);

        return MapToResponse(alert);
    }

    private async Task<RiskAlert> GetAlertAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _alertRepository.GetByIdAsync(
            id, cancellationToken)
            ?? throw new KeyNotFoundException(
                "Alerta de risco não encontrado.");
    }

    private static AlertResponse MapToResponse(RiskAlert alert)
    {
        return new AlertResponse
        {
            Id = alert.Id,
            CreditMonitoringId = alert.CreditMonitoringId,
            Level = alert.Level,
            Status = alert.Status,
            Title = alert.Title,
            Description = alert.Description,
            CreatedAt = alert.CreatedAt,
            UpdatedAt = alert.UpdatedAt,
            ResolvedAt = alert.ResolvedAt
        };
    }
}