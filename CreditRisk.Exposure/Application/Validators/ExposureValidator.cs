
using Exposure.Application.DTOs;

namespace Exposure.Application.Validators;

public class ExposureValidator
{
    public Dictionary<string, string> Validate(ExposureRequest request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados da exposição são obrigatórios.");
            return errors;
        }

        if (request.CreditOperationId == Guid.Empty)
            errors.Add("CreditOperationId", "A operação de crédito é obrigatória.");

        if (request.CreditLimitId == Guid.Empty)
            errors.Add("CreditLimitId", "O limite de crédito é obrigatório.");

        if (request.Amount <= 0)
            errors.Add("Amount", "O valor da exposição deve ser maior que zero.");

        return errors;
    }

    public Dictionary<string, string> ValidateAnalysis(
        ExposureAnalysisRequest request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados da análise são obrigatórios.");
            return errors;
        }

        if (request.CreditLimitId == Guid.Empty)
            errors.Add("CreditLimitId", "O limite de crédito é obrigatório.");

        if (request.AdditionalAmount < 0)
            errors.Add("AdditionalAmount", "O valor adicional não pode ser negativo.");

        if (request.CategoryExposure < 0)
            errors.Add("CategoryExposure", "A exposição da categoria não pode ser negativa.");

        if (request.TotalPortfolioExposure < 0)
            errors.Add("TotalPortfolioExposure", "A exposição total não pode ser negativa.");

        if (request.CategoryExposure > request.TotalPortfolioExposure)
            errors.Add("CategoryExposure", "A exposição da categoria não pode ultrapassar a exposição total.");

        if (request.MaximumConcentrationPercentage <= 0 ||
            request.MaximumConcentrationPercentage > 100)
            errors.Add("MaximumConcentrationPercentage", "A concentração máxima deve estar entre 0 e 100%.");

        return errors;
    }
}
