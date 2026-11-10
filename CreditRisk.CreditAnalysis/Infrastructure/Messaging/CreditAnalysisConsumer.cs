using CreditRisk.CreditAnalysis.Application.DTOs;
using CreditRisk.CreditAnalysis.Application.Services;
using MassTransit;

namespace CreditRisk.CreditAnalysis.Infrastructure.Messaging;

public class CreditAnalysisConsumer : IConsumer<CreditAnalysisRequest>
{
    private readonly ICreditAnalysisService _creditAnalysisService;

    public CreditAnalysisConsumer(ICreditAnalysisService creditAnalysisService)
    {
        _creditAnalysisService = creditAnalysisService;
    }

    public async Task Consume(ConsumeContext<CreditAnalysisRequest> context)
    {
        var result = _creditAnalysisService.Analyze(context.Message);

        await context.RespondAsync<CreditAnalysisResponse>(result);
    }
}