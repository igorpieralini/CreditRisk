using Exposure.Application.DTOs;

namespace Exposure.Application.Services;

public interface IExposureAnalysisService
{
    Task<ExposureAnalysisResponse> AnalyzeAsync(
        ExposureAnalysisRequest request,
        CancellationToken cancellationToken = default);
}