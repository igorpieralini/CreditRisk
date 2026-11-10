using Exposure.Application.DTOs;

namespace Exposure.Application.Services;

public interface IExposureService
{
    Task<ExposureResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ExposureResponse?> GetByCreditOperationIdAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExposureResponse>> GetByCreditLimitIdAsync(
        Guid creditLimitId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExposureResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ExposureResponse> CreateAsync(
        ExposureRequest request,
        CancellationToken cancellationToken = default);

    Task<ExposureResponse> IncreaseAsync(
        Guid id,
        decimal amount,
        CancellationToken cancellationToken = default);

    Task<ExposureResponse> ReduceAsync(
        Guid id,
        decimal amount,
        CancellationToken cancellationToken = default);
}