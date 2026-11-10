
using CreditRisk.RiskDecision.Application.DTOs;
using CreditRisk.RiskDecision.Domain.Entities;
using CreditRisk.RiskDecision.Domain.Enums;
using CreditRisk.RiskDecision.Domain.Repositories;

namespace CreditRisk.RiskDecision.Application.Services;

public class RiskDecisionService : IRiskDecisionService
{
    private readonly ICreditDecisionRepository _decisionRepository;
    private readonly IDecisionHistoryRepository _historyRepository;
    private readonly IDecisionPolicyRepository _policyRepository;
    private readonly IRiskEvaluationService _evaluationService;

    public RiskDecisionService(
        ICreditDecisionRepository decisionRepository,
        IDecisionHistoryRepository historyRepository,
        IDecisionPolicyRepository policyRepository,
        IRiskEvaluationService evaluationService)
    {
        _decisionRepository = decisionRepository;
        _historyRepository = historyRepository;
        _policyRepository = policyRepository;
        _evaluationService = evaluationService;
    }

    public async Task<RiskDecisionResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var decision = await _decisionRepository.GetByIdAsync(
            id, cancellationToken);

        return decision is null ? null : MapToResponse(decision);
    }

    public async Task<IReadOnlyList<RiskDecisionResponse>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        var decisions = await _decisionRepository.GetByCreditOperationIdAsync(
            creditOperationId, cancellationToken);

        return decisions.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<RiskDecisionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var decisions = await _decisionRepository.GetAllAsync(
            cancellationToken);

        return decisions.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<DecisionHistoryResponse>> GetHistoryAsync(
        Guid creditDecisionId,
        CancellationToken cancellationToken = default)
    {
        var decision = await _decisionRepository.GetByIdAsync(
            creditDecisionId, cancellationToken);

        if (decision is null)
            throw new KeyNotFoundException(
                "Decisão de crédito não encontrada.");

        var histories = await _historyRepository.GetByCreditDecisionIdAsync(
            creditDecisionId, cancellationToken);

        return histories.Select(h => new DecisionHistoryResponse
        {
            Id = h.Id,
            CreditDecisionId = h.CreditDecisionId,
            Status = h.Status,
            Reason = h.Reason,
            CreatedAt = h.CreatedAt
        }).ToList();
    }

    public async Task<RiskDecisionResponse> EvaluateAsync(
        RiskDecisionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.CreditOperationId == Guid.Empty)
            throw new ArgumentException(
                "A operação de crédito é obrigatória.");

        if (request.RequestedAmount <= 0)
            throw new ArgumentException(
                "O valor solicitado deve ser maior que zero.");

        var policy = await _policyRepository.GetActiveAsync(
            cancellationToken)
            ?? throw new InvalidOperationException(
                "Nenhuma política de decisão ativa foi encontrada.");

        var indicators = await _evaluationService.EvaluateAsync(
            request,
            policy,
            cancellationToken);

        bool scoreApproved = indicators.MeetsCreditScoreRequirement;
        bool guaranteeApproved = indicators.MeetsGuaranteeCoverageRequirement;
        bool exposureApproved = indicators.MeetsExposureLimitRequirement;

        int approvedRules =
            (scoreApproved ? 1 : 0) +
            (guaranteeApproved ? 1 : 0) +
            (exposureApproved ? 1 : 0);

        DecisionStatus status;
        string reason;

        if (approvedRules == 3)
        {
            status = DecisionStatus.Approved;
            reason = "A operação atende a todos os critérios de risco.";
        }
        else if (approvedRules == 2)
        {
            status = DecisionStatus.ManualReview;
            reason = "A operação exige revisão manual por não atender a todos os critérios.";
        }
        else
        {
            status = DecisionStatus.Rejected;
            reason = "A operação não atende aos critérios mínimos de risco.";
        }

        var decision = new CreditDecision(
            request.CreditOperationId,
            request.RequestedAmount,
            indicators.RiskScore,
            indicators.GuaranteeCoveragePercentage,
            indicators.LimitUtilizationPercentage,
            status,
            reason
        );

        await _decisionRepository.AddAsync(
            decision, cancellationToken);

        var history = new DecisionHistory(
            decision.Id,
            status,
            reason
        );

        await _historyRepository.AddAsync(
            history, cancellationToken);

        return MapToResponse(decision);
    }

    private static RiskDecisionResponse MapToResponse(
        CreditDecision decision)
    {
        return new RiskDecisionResponse
        {
            Id = decision.Id,
            CreditOperationId = decision.CreditOperationId,
            RequestedAmount = decision.RequestedAmount,
            RiskScore = decision.RiskScore,
            GuaranteeCoveragePercentage = decision.GuaranteeCoveragePercentage,
            LimitUtilizationPercentage = decision.LimitUtilizationPercentage,
            Status = decision.Status,
            Reason = decision.Reason,
            CreatedAt = decision.CreatedAt
        };
    }
}
