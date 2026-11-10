using CreditRisk.Guarantee.Application.DTOs;

namespace CreditRisk.Guarantee.Application.Services;

public interface IGuaranteeService
{
    Task<GuaranteeResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GuaranteeResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<GuaranteeResponse> CreateAsync(
        GuaranteeRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}