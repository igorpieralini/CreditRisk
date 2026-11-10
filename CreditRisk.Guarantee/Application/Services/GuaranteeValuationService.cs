
using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Domain.Calculations;
using CreditRisk.Guarantee.Domain.Entities;
using CreditRisk.Guarantee.Domain.Repositories;

namespace CreditRisk.Guarantee.Application.Services;

public class GuaranteeValuationService : IGuaranteeValuationService
{
    private readonly IGuaranteeRepository _guaranteeRepository;
    private readonly IGuaranteeValuationRepository _valuationRepository;
    private readonly EligibleValueCalculator _eligibleValueCalculator;

    public GuaranteeValuationService(
        IGuaranteeRepository guaranteeRepository,
        IGuaranteeValuationRepository valuationRepository,
        EligibleValueCalculator eligibleValueCalculator)
    {
        _guaranteeRepository = guaranteeRepository;
        _valuationRepository = valuationRepository;
        _eligibleValueCalculator = eligibleValueCalculator;
    }

    public async Task<GuaranteeValuationResponse> CreateAsync(
        GuaranteeValuationRequest request,
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

        _eligibleValueCalculator.Calculate(
            request.MarketValue,
            request.HaircutPercentage
        );

        var valuation = new GuaranteeValuation(
            request.GuaranteeId,
            request.MarketValue,
            request.HaircutPercentage
        );

        await _valuationRepository.AddAsync(valuation, cancellationToken);

        return MapToResponse(valuation);
    }

    public async Task<GuaranteeValuationResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var valuation = await _valuationRepository.GetByIdAsync(
            id,
            cancellationToken
        );

        return valuation is null ? null : MapToResponse(valuation);
    }

    public async Task<IReadOnlyList<GuaranteeValuationResponse>> GetHistoryAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default)
    {
        var guarantee = await _guaranteeRepository.GetByIdAsync(
            guaranteeId,
            cancellationToken
        );

        if (guarantee is null)
            throw new KeyNotFoundException("Garantia não encontrada.");

        var valuations = await _valuationRepository.GetByGuaranteeIdAsync(
            guaranteeId,
            cancellationToken
        );

        return valuations.Select(MapToResponse).ToList();
    }

    public async Task<GuaranteeValuationResponse?> GetLatestAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default)
    {
        var guarantee = await _guaranteeRepository.GetByIdAsync(
            guaranteeId,
            cancellationToken
        );

        if (guarantee is null)
            throw new KeyNotFoundException("Garantia não encontrada.");

        var valuation = await _valuationRepository.GetLatestByGuaranteeIdAsync(
            guaranteeId,
            cancellationToken
        );

        return valuation is null ? null : MapToResponse(valuation);
    }

    private static GuaranteeValuationResponse MapToResponse(
        GuaranteeValuation valuation)
    {
        return new GuaranteeValuationResponse
        {
            Id = valuation.Id,
            GuaranteeId = valuation.GuaranteeId,
            MarketValue = valuation.MarketValue,
            HaircutPercentage = valuation.HaircutPercentage,
            EligibleValue = valuation.EligibleValue,
            ValuationDate = valuation.ValuationDate
        };
    }
}