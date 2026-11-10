using CreditRisk.Monitoring.Application.DTOs;
using CreditRisk.Monitoring.Application.Services;
using MassTransit;

namespace CreditRisk.Monitoring.Infrastructure.Messaging;

public class MonitoringEventConsumer : IConsumer<MonitoringEventRequest>
{
    private readonly IMonitoringService _monitoringService;

    public MonitoringEventConsumer(IMonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    public async Task Consume(
        ConsumeContext<MonitoringEventRequest> context)
    {
        await _monitoringService.RegisterEventAsync(
            context.Message,
            context.CancellationToken
        );
    }
}