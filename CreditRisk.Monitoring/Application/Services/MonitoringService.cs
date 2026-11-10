
using CreditRisk.Monitoring.Application.DTOs;
using CreditRisk.Monitoring.Domain.Entities;
using CreditRisk.Monitoring.Domain.Enums;
using CreditRisk.Monitoring.Domain.Repositories;
using CreditRisk.Monitoring.Domain.Rules;

namespace CreditRisk.Monitoring.Application.Services;

public class MonitoringService : IMonitoringService
{
    private readonly ICreditMonitoringRepository _monitoringRepository;
    private readonly IRiskAlertRepository _alertRepository;
    private readonly IMonitoringEventRepository _eventRepository;

    private readonly RiskScoreAlertRule _riskScoreRule;
    private readonly ExposureAlertRule _exposureRule;
    private readonly GuaranteeAlertRule _guaranteeRule;

    public MonitoringService(
        ICreditMonitoringRepository monitoringRepository,
        IRiskAlertRepository alertRepository,
        IMonitoringEventRepository eventRepository,
        RiskScoreAlertRule riskScoreRule,
        ExposureAlertRule exposureRule,
        GuaranteeAlertRule guaranteeRule)
    {
        _monitoringRepository = monitoringRepository;
        _alertRepository = alertRepository;
        _eventRepository = eventRepository;

        _riskScoreRule = riskScoreRule;
        _exposureRule = exposureRule;
        _guaranteeRule = guaranteeRule;
    }

    public async Task<MonitoringResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var monitoring = await _monitoringRepository.GetByIdAsync(
            id, cancellationToken);

        return monitoring is null ? null : MapToResponse(monitoring);
    }

    public async Task<MonitoringResponse?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        var monitoring = await _monitoringRepository.GetByCreditOperationIdAsync(
            creditOperationId, cancellationToken);

        return monitoring is null ? null : MapToResponse(monitoring);
    }

    public async Task<IReadOnlyList<MonitoringResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var monitorings = await _monitoringRepository.GetAllAsync(
            cancellationToken);

        return monitorings.Select(MapToResponse).ToList();
    }

    public async Task<MonitoringResponse> CreateAsync(
        MonitoringRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existing = await _monitoringRepository.GetByCreditOperationIdAsync(
            request.CreditOperationId, cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                "A operação de crédito já possui um monitoramento.");

        var monitoring = new CreditMonitoring(
            request.CreditOperationId,
            request.RiskScore,
            request.GuaranteeCoveragePercentage,
            request.LimitUtilizationPercentage
        );

        await _monitoringRepository.AddAsync(
            monitoring, cancellationToken);

        await EvaluateAlertsAsync(
            monitoring, cancellationToken);

        return MapToResponse(monitoring);
    }

    public async Task<MonitoringResponse> UpdateAsync(
        Guid id,
        MonitoringRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var monitoring = await _monitoringRepository.GetByIdAsync(
            id, cancellationToken)
            ?? throw new KeyNotFoundException(
                "Monitoramento não encontrado.");

        if (monitoring.CreditOperationId != request.CreditOperationId)
            throw new InvalidOperationException(
                "Não é permitido alterar a operação de crédito do monitoramento.");

        monitoring.UpdateIndicators(
            request.RiskScore,
            request.GuaranteeCoveragePercentage,
            request.LimitUtilizationPercentage
        );

        await _monitoringRepository.UpdateAsync(
            monitoring, cancellationToken);

        await EvaluateAlertsAsync(
            monitoring, cancellationToken);

        return MapToResponse(monitoring);
    }

    public async Task RegisterEventAsync(
        MonitoringEventRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!Enum.IsDefined(request.Type))
            throw new ArgumentException(
                "O tipo de evento é inválido.");

        var monitoring = await _monitoringRepository.GetByCreditOperationIdAsync(
            request.CreditOperationId, cancellationToken)
            ?? throw new KeyNotFoundException(
                "Nenhum monitoramento foi encontrado para a operação.");

        var monitoringEvent = new MonitoringEvent(
            monitoring.Id,
            request.Type,
            request.Description
        );

        await _eventRepository.AddAsync(
            monitoringEvent, cancellationToken);
    }

    private async Task EvaluateAlertsAsync(
        CreditMonitoring monitoring,
        CancellationToken cancellationToken)
    {
        var existingAlerts = await _alertRepository.GetByCreditMonitoringIdAsync(
            monitoring.Id, cancellationToken);

        await CreateAlertIfNecessaryAsync(
            monitoring.Id,
            _riskScoreRule.Evaluate(monitoring.RiskScore),
            "Score de risco",
            $"O score atual da operação é {monitoring.RiskScore:N2}.",
            existingAlerts,
            cancellationToken);

        await CreateAlertIfNecessaryAsync(
            monitoring.Id,
            _exposureRule.Evaluate(monitoring.LimitUtilizationPercentage),
            "Utilização do limite",
            $"A utilização atual do limite é {monitoring.LimitUtilizationPercentage:N2}%.",
            existingAlerts,
            cancellationToken);

        await CreateAlertIfNecessaryAsync(
            monitoring.Id,
            _guaranteeRule.Evaluate(monitoring.GuaranteeCoveragePercentage),
            "Cobertura de garantias",
            $"A cobertura atual das garantias é {monitoring.GuaranteeCoveragePercentage:N2}%.",
            existingAlerts,
            cancellationToken);
    }

    private async Task CreateAlertIfNecessaryAsync(
        Guid monitoringId,
        AlertLevel? level,
        string title,
        string description,
        IReadOnlyList<RiskAlert> existingAlerts,
        CancellationToken cancellationToken)
    {
        if (level is null)
            return;

        bool alreadyExists = existingAlerts.Any(a =>
            a.Title == title &&
            a.Status != AlertStatus.Resolved);

        if (alreadyExists)
            return;

        var alert = new RiskAlert(
            monitoringId,
            level.Value,
            title,
            description
        );

        await _alertRepository.AddAsync(
            alert, cancellationToken);
    }

    private static MonitoringResponse MapToResponse(
        CreditMonitoring monitoring)
    {
        return new MonitoringResponse
        {
            Id = monitoring.Id,
            CreditOperationId = monitoring.CreditOperationId,
            RiskScore = monitoring.RiskScore,
            GuaranteeCoveragePercentage = monitoring.GuaranteeCoveragePercentage,
            LimitUtilizationPercentage = monitoring.LimitUtilizationPercentage,
            CreatedAt = monitoring.CreatedAt,
            UpdatedAt = monitoring.UpdatedAt
        };
    }
}