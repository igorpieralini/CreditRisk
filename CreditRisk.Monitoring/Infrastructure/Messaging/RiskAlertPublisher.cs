using CreditRisk.Monitoring.Application.DTOs;
using MassTransit;

namespace CreditRisk.Monitoring.Infrastructure.Messaging;

public class RiskAlertPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RiskAlertPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(
        AlertResponse alert,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(alert);

        await _publishEndpoint.Publish(
            alert,
            cancellationToken);
    }
}