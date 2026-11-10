
using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Domain.Calculations;
using CreditRisk.Guarantee.Domain.Repositories;
using CreditRisk.Guarantee.Domain.Rules;

namespace CreditRisk.Guarantee.Application.Services;

public class GuaranteeAnalysisService : IGuaranteeAnalysisService
{
    private readonly IGuaranteeRepository _guaranteeRepository;
    private readonly LtvCalculator _ltvCalculator;
    private readonly CoverageCalculator _coverageCalculator;
    private readonly EligibleValueCalculator _eligibleValueCalculator;
    private readonly GuaranteeSufficiencyRule _sufficiencyRule;
    private readonly MaximumLtvRule _maximumLtvRule;

    public GuaranteeAnalysisService(
        IGuaranteeRepository guaranteeRepository,
        LtvCalculator ltvCalculator,
        CoverageCalculator coverageCalculator,
        EligibleValueCalculator eligibleValueCalculator,
        GuaranteeSufficiencyRule sufficiencyRule,
        MaximumLtvRule maximumLtvRule)
    {
        _guaranteeRepository = guaranteeRepository;
        _ltvCalculator = ltvCalculator;
        _coverageCalculator = coverageCalculator;
        _eligibleValueCalculator = eligibleValueCalculator;
        _sufficiencyRule = sufficiencyRule;
        _maximumLtvRule = maximumLtvRule;
    }

    public async Task<GuaranteeAnalysisResponse> AnalyzeAsync(
        GuaranteeAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var guarantee = await _guaranteeRepository.GetByIdAsync(
            request.GuaranteeId,
            cancellationToken
        );

        if (guarantee is null)
            throw new KeyNotFoundException("Garantia não encontrada.");

        if (!guarantee.IsActive)
            throw new InvalidOperationException("A garantia está inativa.");

        if (request.CreditOperationId == Guid.Empty)
            throw new ArgumentException("A operação de crédito é obrigatória.");

        decimal eligibleValue = _eligibleValueCalculator.Calculate(
            request.MarketValue,
            request.HaircutPercentage
        );

        decimal ltv = _ltvCalculator.Calculate(
            request.OperationAmount,
            request.MarketValue
        );

        decimal coverage = _coverageCalculator.Calculate(
            eligibleValue,
            request.OperationAmount
        );

        bool isSufficient = _sufficiencyRule.Validate(
            eligibleValue,
            request.OperationAmount
        );

        bool isWithinMaximumLtv = _maximumLtvRule.Validate(
            ltv,
            request.MaximumLtv
        );

        return new GuaranteeAnalysisResponse
        {
            GuaranteeId = guarantee.Id,
            CreditOperationId = request.CreditOperationId,
            OperationAmount = request.OperationAmount,
            MarketValue = request.MarketValue,
            EligibleValue = eligibleValue,
            HaircutPercentage = request.HaircutPercentage,
            Ltv = ltv,
            CoveragePercentage = coverage,
            IsSufficient = isSufficient,
            IsWithinMaximumLtv = isWithinMaximumLtv
        };
    }
}
