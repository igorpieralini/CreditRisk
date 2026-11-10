
using Exposure.Application.DTOs;
using Exposure.Domain.Calculations;
using Exposure.Domain.Repositories;
using Exposure.Domain.Rules;

namespace Exposure.Application.Services;

public class ExposureAnalysisService : IExposureAnalysisService
{
    private readonly ICreditLimitRepository _limitRepository;
    private readonly ICreditExposureRepository _exposureRepository;
    private readonly ExposureCalculator _exposureCalculator;
    private readonly LimitUtilizationCalculator _utilizationCalculator;
    private readonly ConcentrationCalculator _concentrationCalculator;
    private readonly CreditLimitRule _creditLimitRule;
    private readonly ExposureConcentrationRule _concentrationRule;

    public ExposureAnalysisService(
        ICreditLimitRepository limitRepository,
        ICreditExposureRepository exposureRepository,
        ExposureCalculator exposureCalculator,
        LimitUtilizationCalculator utilizationCalculator,
        ConcentrationCalculator concentrationCalculator,
        CreditLimitRule creditLimitRule,
        ExposureConcentrationRule concentrationRule)
    {
        _limitRepository = limitRepository;
        _exposureRepository = exposureRepository;
        _exposureCalculator = exposureCalculator;
        _utilizationCalculator = utilizationCalculator;
        _concentrationCalculator = concentrationCalculator;
        _creditLimitRule = creditLimitRule;
        _concentrationRule = concentrationRule;
    }

    public async Task<ExposureAnalysisResponse> AnalyzeAsync(
        ExposureAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.AdditionalAmount < 0)
            throw new ArgumentException("O valor adicional não pode ser negativo.");

        if (request.CategoryExposure < 0)
            throw new ArgumentException("A exposição da categoria não pode ser negativa.");

        if (request.TotalPortfolioExposure < 0)
            throw new ArgumentException("A exposição da carteira não pode ser negativa.");

        var limit = await _limitRepository.GetByIdAsync(
            request.CreditLimitId,
            cancellationToken
        ) ?? throw new KeyNotFoundException("Limite de crédito não encontrado.");

        if (!limit.IsActive)
            throw new InvalidOperationException("O limite de crédito está inativo.");

        var exposures = await _exposureRepository.GetByCreditLimitIdAsync(
            limit.Id,
            cancellationToken
        );

        decimal currentExposure = _exposureCalculator.CalculateTotalExposure(
            exposures.Select(e => e.OutstandingAmount)
        );

        decimal projectedExposure = currentExposure + request.AdditionalAmount;

        decimal availableLimit = _exposureCalculator.CalculateAvailableLimit(
            limit.TotalAmount,
            projectedExposure
        );

        decimal utilization = _utilizationCalculator.Calculate(
            projectedExposure,
            limit.TotalAmount
        );

        decimal projectedCategoryExposure =
            request.CategoryExposure + request.AdditionalAmount;

        decimal projectedPortfolioExposure =
            request.TotalPortfolioExposure + request.AdditionalAmount;

        decimal concentration = _concentrationCalculator.Calculate(
            projectedCategoryExposure,
            projectedPortfolioExposure
        );

        bool withinCreditLimit = _creditLimitRule.Validate(
            projectedExposure,
            limit.TotalAmount
        );

        bool withinConcentrationLimit = _concentrationRule.Validate(
            concentration,
            request.MaximumConcentrationPercentage
        );

        return new ExposureAnalysisResponse
        {
            CreditLimitId = limit.Id,
            TotalLimit = limit.TotalAmount,
            CurrentExposure = currentExposure,
            AdditionalAmount = request.AdditionalAmount,
            ProjectedExposure = projectedExposure,
            AvailableLimit = availableLimit,
            UtilizationPercentage = utilization,
            ConcentrationPercentage = concentration,
            IsWithinCreditLimit = withinCreditLimit,
            IsWithinConcentrationLimit = withinConcentrationLimit
        };
    }
}
