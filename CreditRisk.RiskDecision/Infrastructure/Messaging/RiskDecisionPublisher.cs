using CreditRisk.RiskDecision.Application.DTOs;
using MassTransit;

namespace CreditRisk.RiskDecision.Infrastructure.Messaging;

public class RiskDecisionPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RiskDecisionPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(
        RiskDecisionResponse decision,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(decision);

        await _publishEndpoint.Publish(
            decision,
            cancellationToken
        );
    }
}