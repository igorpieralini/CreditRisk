
using Exposure.Application.DTOs;
using Exposure.Domain.Entities;
using Exposure.Domain.Enums;
using Exposure.Domain.Repositories;
using Exposure.Domain.Rules;

namespace Exposure.Application.Services;

public class ExposureService : IExposureService
{
    private readonly ICreditExposureRepository _exposureRepository;
    private readonly ICreditLimitRepository _limitRepository;
    private readonly IExposureMovementRepository _movementRepository;
    private readonly CreditLimitRule _creditLimitRule;

    public ExposureService(
        ICreditExposureRepository exposureRepository,
        ICreditLimitRepository limitRepository,
        IExposureMovementRepository movementRepository,
        CreditLimitRule creditLimitRule)
    {
        _exposureRepository = exposureRepository;
        _limitRepository = limitRepository;
        _movementRepository = movementRepository;
        _creditLimitRule = creditLimitRule;
    }

    public async Task<ExposureResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exposure = await _exposureRepository.GetByIdAsync(
            id, cancellationToken);

        return exposure is null ? null : MapToResponse(exposure);
    }

    public async Task<ExposureResponse?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        var exposure = await _exposureRepository.GetByCreditOperationIdAsync(
            creditOperationId, cancellationToken);

        return exposure is null ? null : MapToResponse(exposure);
    }

    public async Task<IReadOnlyList<ExposureResponse>> GetByCreditLimitIdAsync(
        Guid creditLimitId,
        CancellationToken cancellationToken = default)
    {
        var exposures = await _exposureRepository.GetByCreditLimitIdAsync(
            creditLimitId, cancellationToken);

        return exposures.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<ExposureResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var exposures = await _exposureRepository.GetAllAsync(cancellationToken);

        return exposures.Select(MapToResponse).ToList();
    }

    public async Task<ExposureResponse> CreateAsync(
        ExposureRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.CreditOperationId == Guid.Empty)
            throw new ArgumentException("A operação de crédito é obrigatória.");

        if (request.Amount <= 0)
            throw new ArgumentException("O valor da exposição deve ser maior que zero.");

        var existing = await _exposureRepository.GetByCreditOperationIdAsync(
            request.CreditOperationId, cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                "A operação de crédito já possui uma exposição.");

        var limit = await GetActiveLimitAsync(
            request.CreditLimitId, cancellationToken);

        await ValidateAvailableLimitAsync(
            limit, request.Amount, cancellationToken);

        var exposure = new CreditExposure(
            request.CreditOperationId,
            limit.Id,
            request.Amount
        );

        await _exposureRepository.AddAsync(exposure, cancellationToken);

        var movement = new ExposureMovement(
            exposure.Id,
            ExposureMovementType.Increase,
            request.Amount,
            0m,
            request.Amount
        );

        await _movementRepository.AddAsync(movement, cancellationToken);

        return MapToResponse(exposure);
    }

    public async Task<ExposureResponse> IncreaseAsync(
        Guid id,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var exposure = await GetExposureAsync(id, cancellationToken);

        var limit = await GetActiveLimitAsync(
            exposure.CreditLimitId, cancellationToken);

        await ValidateAvailableLimitAsync(
            limit, amount, cancellationToken);

        decimal previousBalance = exposure.OutstandingAmount;

        exposure.Increase(amount);

        var movement = new ExposureMovement(
            exposure.Id,
            ExposureMovementType.Increase,
            amount,
            previousBalance,
            exposure.OutstandingAmount
        );

        await _exposureRepository.UpdateAsync(exposure, cancellationToken);
        await _movementRepository.AddAsync(movement, cancellationToken);

        return MapToResponse(exposure);
    }

    public async Task<ExposureResponse> ReduceAsync(
        Guid id,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var exposure = await GetExposureAsync(id, cancellationToken);

        decimal previousBalance = exposure.OutstandingAmount;

        exposure.Reduce(amount);

        var movement = new ExposureMovement(
            exposure.Id,
            ExposureMovementType.Reduction,
            amount,
            previousBalance,
            exposure.OutstandingAmount
        );

        await _exposureRepository.UpdateAsync(exposure, cancellationToken);
        await _movementRepository.AddAsync(movement, cancellationToken);

        return MapToResponse(exposure);
    }

    private async Task<CreditExposure> GetExposureAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _exposureRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Exposição não encontrada.");
    }

    private async Task<CreditLimit> GetActiveLimitAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var limit = await _limitRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Limite de crédito não encontrado.");

        if (!limit.IsActive)
            throw new InvalidOperationException("O limite de crédito está inativo.");

        return limit;
    }

    private async Task ValidateAvailableLimitAsync(
        CreditLimit limit,
        decimal amount,
        CancellationToken cancellationToken)
    {
        var exposures = await _exposureRepository.GetByCreditLimitIdAsync(
            limit.Id, cancellationToken);

        decimal totalExposure = exposures.Sum(e => e.OutstandingAmount);

        if (!_creditLimitRule.CanIncrease(
                totalExposure, amount, limit.TotalAmount))
        {
            throw new InvalidOperationException(
                "A operação ultrapassa o limite de crédito disponível.");
        }
    }

    private static ExposureResponse MapToResponse(CreditExposure exposure)
    {
        return new ExposureResponse
        {
            Id = exposure.Id,
            CreditOperationId = exposure.CreditOperationId,
            CreditLimitId = exposure.CreditLimitId,
            OriginalAmount = exposure.OriginalAmount,
            OutstandingAmount = exposure.OutstandingAmount,
            Status = exposure.Status,
            CreatedAt = exposure.CreatedAt,
            UpdatedAt = exposure.UpdatedAt
        };
    }
}
