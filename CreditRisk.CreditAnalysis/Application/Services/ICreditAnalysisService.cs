using CreditRisk.CreditAnalysis.Application.DTOs;

namespace CreditRisk.CreditAnalysis.Application.Services;

public interface ICreditAnalysisService
{
    CreditAnalysisResponse Analyze(CreditAnalysisRequest request);
}