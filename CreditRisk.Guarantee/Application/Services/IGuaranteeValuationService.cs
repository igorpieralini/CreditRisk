using CreditRisk.Guarantee.Application.DTOs;

namespace CreditRisk.Guarantee.Application.Services;

public interface IGuaranteeValuationService
{
    Task<GuaranteeValuationResponse> CreateAsync(
        GuaranteeValuationRequest request,
        CancellationToken cancellationToken = default);

    Task<GuaranteeValuationResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeValuationResponse>> GetHistoryAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default);

    Task<GuaranteeValuationResponse?> GetLatestAsync(
        Guid guaranteeId,
        CancellationToken cancellationToken = default);
}