
using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Application.Services;
using CreditRisk.Guarantee.Domain.Repositories;
using GuaranteeEntity = global::CreditRisk.Guarantee.Domain.Entities.Guarantee;

namespace CreditRisk.Guarantee.Application.Services;

public class GuaranteeService : IGuaranteeService
{
    private readonly IGuaranteeRepository _guaranteeRepository;

    public GuaranteeService(IGuaranteeRepository guaranteeRepository)
    {
        _guaranteeRepository = guaranteeRepository;
    }

    public async Task<GuaranteeResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var guarantee = await _guaranteeRepository.GetByIdAsync(
            id,
            cancellationToken
        );

        return guarantee is null ? null : MapToResponse(guarantee);
    }

    public async Task<IReadOnlyList<GuaranteeResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var guarantees = await _guaranteeRepository.GetAllAsync(cancellationToken);

        return guarantees.Select(MapToResponse).ToList();
    }

    public async Task<GuaranteeResponse> CreateAsync(
        GuaranteeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var guarantee = new GuaranteeEntity(
            request.Type,
            request.Description,
            request.OriginalValue
        );

        await _guaranteeRepository.AddAsync(guarantee, cancellationToken);

        return MapToResponse(guarantee);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var guarantee = await _guaranteeRepository.GetByIdAsync(
            id,
            cancellationToken
        );

        if (guarantee is null)
            throw new KeyNotFoundException("Garantia não encontrada.");

        if (!guarantee.IsActive)
            throw new InvalidOperationException("A garantia já está desativada.");

        guarantee.Deactivate();

        await _guaranteeRepository.UpdateAsync(guarantee, cancellationToken);
    }

    private static GuaranteeResponse MapToResponse(GuaranteeEntity guarantee)
    {
        return new GuaranteeResponse
        {
            Id = guarantee.Id,
            Type = guarantee.Type,
            Description = guarantee.Description,
            OriginalValue = guarantee.OriginalValue,
            CreatedAt = guarantee.CreatedAt,
            IsActive = guarantee.IsActive
        };
    }
}
