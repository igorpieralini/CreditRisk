using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Application.Services;
using MassTransit;

namespace CreditRisk.Guarantee.Infrastructure.Messaging;

public class GuaranteeAnalysisConsumer : IConsumer<GuaranteeAnalysisRequest>
{
    private readonly IGuaranteeAnalysisService _guaranteeAnalysisService;

    public GuaranteeAnalysisConsumer(
        IGuaranteeAnalysisService guaranteeAnalysisService)
    {
        _guaranteeAnalysisService = guaranteeAnalysisService;
    }

    public async Task Consume(
        ConsumeContext<GuaranteeAnalysisRequest> context)
    {
        var result = await _guaranteeAnalysisService.AnalyzeAsync(
            context.Message,
            context.CancellationToken
        );

        await context.RespondAsync<GuaranteeAnalysisResponse>(result);
    }
}