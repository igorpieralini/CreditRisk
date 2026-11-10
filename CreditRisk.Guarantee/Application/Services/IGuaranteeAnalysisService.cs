using CreditRisk.Guarantee.Application.DTOs;

namespace CreditRisk.Guarantee.Application.Services;

public interface IGuaranteeAnalysisService
{
    Task<GuaranteeAnalysisResponse> AnalyzeAsync(
        GuaranteeAnalysisRequest request,
        CancellationToken cancellationToken = default);
}