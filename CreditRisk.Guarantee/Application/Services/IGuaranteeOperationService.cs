using CreditRisk.Guarantee.Application.DTOs;

namespace CreditRisk.Guarantee.Application.Services;

public interface IGuaranteeOperationService
{
    Task<GuaranteeOperationResponse> CreateAsync(
        GuaranteeOperationRequest request,
        CancellationToken cancellationToken = default);

    Task<GuaranteeOperationResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeOperationResponse>> GetByGuaranteeIdAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeOperationResponse>> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task ReleaseAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}