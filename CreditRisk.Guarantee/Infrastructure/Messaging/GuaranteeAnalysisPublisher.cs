using CreditRisk.Guarantee.Application.DTOs;
using MassTransit;

namespace CreditRisk.Guarantee.Infrastructure.Messaging;

public class GuaranteeAnalysisPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public GuaranteeAnalysisPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(
        GuaranteeAnalysisResponse response,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(response);

        await _publishEndpoint.Publish(
            response,
            cancellationToken
        );
    }
}