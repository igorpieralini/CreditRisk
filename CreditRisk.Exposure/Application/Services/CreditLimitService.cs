
using Exposure.Application.DTOs;
using Exposure.Domain.Entities;
using Exposure.Domain.Repositories;

namespace Exposure.Application.Services;

public class CreditLimitService : ICreditLimitService
{
    private readonly ICreditLimitRepository _limitRepository;
    private readonly ICreditExposureRepository _exposureRepository;

    public CreditLimitService(
        ICreditLimitRepository limitRepository,
        ICreditExposureRepository exposureRepository)
    {
        _limitRepository = limitRepository;
        _exposureRepository = exposureRepository;
    }

    public async Task<CreditLimitResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var limit = await _limitRepository.GetByIdAsync(
            id, cancellationToken);

        return limit is null ? null : MapToResponse(limit);
    }

    public async Task<IReadOnlyList<CreditLimitResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var limits = await _limitRepository.GetAllAsync(cancellationToken);

        return limits.Select(MapToResponse).ToList();
    }

    public async Task<CreditLimitResponse> CreateAsync(
        CreditLimitRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var limit = new CreditLimit(
            request.Name,
            request.TotalAmount
        );

        await _limitRepository.AddAsync(limit, cancellationToken);

        return MapToResponse(limit);
    }

    public async Task<CreditLimitResponse> UpdateAsync(
        Guid id,
        CreditLimitRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var limit = await GetLimitAsync(id, cancellationToken);

        var exposures = await _exposureRepository.GetByCreditLimitIdAsync(
            id, cancellationToken);

        decimal totalExposure = exposures.Sum(e => e.OutstandingAmount);

        if (request.TotalAmount < totalExposure)
            throw new InvalidOperationException(
                "O novo limite não pode ser inferior à exposição atual.");

        limit.UpdateLimit(request.TotalAmount);

        await _limitRepository.UpdateAsync(limit, cancellationToken);

        return MapToResponse(limit);
    }

    public async Task DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var limit = await GetLimitAsync(id, cancellationToken);

        var exposures = await _exposureRepository.GetByCreditLimitIdAsync(
            id, cancellationToken);

        if (exposures.Any(e => e.OutstandingAmount > 0))
            throw new InvalidOperationException(
                "Não é possível desativar um limite com exposição pendente.");

        limit.Deactivate();

        await _limitRepository.UpdateAsync(limit, cancellationToken);
    }

    public async Task ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var limit = await GetLimitAsync(id, cancellationToken);

        limit.Activate();

        await _limitRepository.UpdateAsync(limit, cancellationToken);
    }

    private async Task<CreditLimit> GetLimitAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _limitRepository.GetByIdAsync(
            id, cancellationToken)
            ?? throw new KeyNotFoundException(
                "Limite de crédito não encontrado.");
    }

    private static CreditLimitResponse MapToResponse(CreditLimit limit)
    {
        return new CreditLimitResponse
        {
            Id = limit.Id,
            Name = limit.Name,
            TotalAmount = limit.TotalAmount,
            IsActive = limit.IsActive,
            CreatedAt = limit.CreatedAt,
            UpdatedAt = limit.UpdatedAt
        };
    }
}
