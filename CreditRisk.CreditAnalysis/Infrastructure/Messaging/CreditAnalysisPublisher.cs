using CreditRisk.CreditAnalysis.Application.DTOs;
using MassTransit;

namespace CreditRisk.CreditAnalysis.Infrastructure.Messaging;

public class CreditAnalysisPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CreditAnalysisPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(
        CreditAnalysisResponse response,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(response);

        await _publishEndpoint.Publish(
            response,
            cancellationToken
        );
    }
}