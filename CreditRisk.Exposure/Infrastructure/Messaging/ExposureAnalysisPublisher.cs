using Exposure.Application.DTOs;
using MassTransit;

namespace Exposure.Infrastructure.Messaging;

public class ExposureAnalysisPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ExposureAnalysisPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(
        ExposureAnalysisResponse response,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(response);

        await _publishEndpoint.Publish(
            response,
            cancellationToken
        );
    }
}