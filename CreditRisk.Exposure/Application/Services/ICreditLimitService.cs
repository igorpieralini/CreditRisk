using Exposure.Application.DTOs;

namespace Exposure.Application.Services;

public interface ICreditLimitService
{
    Task<CreditLimitResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CreditLimitResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<CreditLimitResponse> CreateAsync(
        CreditLimitRequest request,
        CancellationToken cancellationToken = default);

    Task<CreditLimitResponse> UpdateAsync(
        Guid id,
        CreditLimitRequest request,
        CancellationToken cancellationToken = default);

    Task DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}