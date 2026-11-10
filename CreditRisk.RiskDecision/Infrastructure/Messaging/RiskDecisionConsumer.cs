using CreditRisk.RiskDecision.Application.DTOs;
using CreditRisk.RiskDecision.Application.Services;
using MassTransit;

namespace CreditRisk.RiskDecision.Infrastructure.Messaging;

public class RiskDecisionConsumer : IConsumer<RiskDecisionRequest>
{
    private readonly IRiskDecisionService _decisionService;

    public RiskDecisionConsumer(IRiskDecisionService decisionService)
    {
        _decisionService = decisionService;
    }

    public async Task Consume(ConsumeContext<RiskDecisionRequest> context)
    {
        var result = await _decisionService.EvaluateAsync(
            context.Message,
            context.CancellationToken
        );

        await context.RespondAsync<RiskDecisionResponse>(result);
    }
}