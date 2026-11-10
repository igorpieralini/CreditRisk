using Exposure.Application.DTOs;
using Exposure.Application.Services;
using MassTransit;

namespace Exposure.Infrastructure.Messaging;

public class ExposureAnalysisConsumer : IConsumer<ExposureAnalysisRequest>
{
    private readonly IExposureAnalysisService _analysisService;

    public ExposureAnalysisConsumer(
        IExposureAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    public async Task Consume(
        ConsumeContext<ExposureAnalysisRequest> context)
    {
        var result = await _analysisService.AnalyzeAsync(
            context.Message,
            context.CancellationToken
        );

        await context.RespondAsync<ExposureAnalysisResponse>(result);
    }
}