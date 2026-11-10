
using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Domain.Entities;
using CreditRisk.Guarantee.Domain.Repositories;

namespace CreditRisk.Guarantee.Application.Services;

public class GuaranteeOperationService : IGuaranteeOperationService
{
    private readonly IGuaranteeRepository _guaranteeRepository;
    private readonly IGuaranteeValuationRepository _valuationRepository;
    private readonly IGuaranteeOperationRepository _operationRepository;

    public GuaranteeOperationService(
        IGuaranteeRepository guaranteeRepository,
        IGuaranteeValuationRepository valuationRepository,
        IGuaranteeOperationRepository operationRepository)
    {
        _guaranteeRepository = guaranteeRepository;
        _valuationRepository = valuationRepository;
        _operationRepository = operationRepository;
    }

    public async Task<GuaranteeOperationResponse> CreateAsync(
        GuaranteeOperationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.CreditOperationId == Guid.Empty)
            throw new ArgumentException("A operação de crédito é obrigatória.");

        if (request.AllocatedValue <= 0)
            throw new ArgumentException("O valor alocado deve ser maior que zero.");

        var guarantee = await _guaranteeRepository.GetByIdAsync(
            request.GuaranteeId,
            cancellationToken
        );

        if (guarantee is null)
            throw new KeyNotFoundException("Garantia não encontrada.");

        if (!guarantee.IsActive)
            throw new InvalidOperationException("A garantia está inativa.");

        var valuation = await _valuationRepository.GetLatestByGuaranteeIdAsync(
            request.GuaranteeId,
            cancellationToken
        );

        if (valuation is null)
            throw new InvalidOperationException("A garantia não possui avaliação.");

        var operations = await _operationRepository.GetByGuaranteeIdAsync(
            request.GuaranteeId,
            cancellationToken
        );

        decimal allocatedTotal = operations
            .Where(o => o.IsActive)
            .Sum(o => o.AllocatedValue);

        decimal availableValue = valuation.EligibleValue - allocatedTotal;

        if (request.AllocatedValue > availableValue)
            throw new InvalidOperationException(
                "O valor solicitado ultrapassa o valor elegível disponível da garantia."
            );

        var operation = new GuaranteeOperation(
            request.GuaranteeId,
            request.CreditOperationId,
            request.AllocatedValue
        );

        await _operationRepository.AddAsync(operation, cancellationToken);

        return MapToResponse(operation);
    }

    public async Task<GuaranteeOperationResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var operation = await _operationRepository.GetByIdAsync(
            id,
            cancellationToken
        );

        return operation is null ? null : MapToResponse(operation);
    }

    public async Task<IReadOnlyList<GuaranteeOperationResponse>> GetByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default)
    {
        var operations = await _operationRepository.GetByGuaranteeIdAsync(
            guaranteeId,
            cancellationToken
        );

        return operations.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<GuaranteeOperationResponse>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        var operations = await _operationRepository.GetByCreditOperationIdAsync(
            creditOperationId,
            cancellationToken
        );

        return operations.Select(MapToResponse).ToList();
    }

    public async Task ReleaseAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var operation = await _operationRepository.GetByIdAsync(
            id,
            cancellationToken
        );

        if (operation is null)
            throw new KeyNotFoundException("Vínculo de garantia não encontrado.");

        operation.Release();

        await _operationRepository.UpdateAsync(
            operation,
            cancellationToken
        );
    }

    private static GuaranteeOperationResponse MapToResponse(
        GuaranteeOperation operation)
    {
        return new GuaranteeOperationResponse
        {
            Id = operation.Id,
            GuaranteeId = operation.GuaranteeId,
            CreditOperationId = operation.CreditOperationId,
            AllocatedValue = operation.AllocatedValue,
            LinkedAt = operation.LinkedAt,
            ReleasedAt = operation.ReleasedAt,
            IsActive = operation.IsActive
        };
    }
}
