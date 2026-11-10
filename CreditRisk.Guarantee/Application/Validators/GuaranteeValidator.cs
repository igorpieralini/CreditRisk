
using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Domain.Enums;

namespace CreditRisk.Guarantee.Application.Validators;

public class GuaranteeValidator
{
    public Dictionary<string, string> Validate(GuaranteeRequest request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados da garantia são obrigatórios.");
            return errors;
        }

        if (!Enum.IsDefined(request.Type))
            errors.Add("Type", "O tipo de garantia é inválido.");

        if (string.IsNullOrWhiteSpace(request.Description))
            errors.Add("Description", "A descrição da garantia é obrigatória.");

        if (request.OriginalValue <= 0)
            errors.Add("OriginalValue", "O valor da garantia deve ser maior que zero.");

        return errors;
    }

    public Dictionary<string, string> ValidateAnalysis(
        GuaranteeAnalysisRequest request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados da análise são obrigatórios.");
            return errors;
        }

        if (request.GuaranteeId == Guid.Empty)
            errors.Add("GuaranteeId", "A garantia é obrigatória.");

        if (request.CreditOperationId == Guid.Empty)
            errors.Add("CreditOperationId", "A operação de crédito é obrigatória.");

        if (request.OperationAmount <= 0)
            errors.Add("OperationAmount", "O valor da operação deve ser maior que zero.");

        if (request.MarketValue <= 0)
            errors.Add("MarketValue", "O valor de mercado deve ser maior que zero.");

        if (request.HaircutPercentage < 0 || request.HaircutPercentage > 100)
            errors.Add("HaircutPercentage", "O haircut deve estar entre 0 e 100%.");

        if (request.MaximumLtv <= 0)
            errors.Add("MaximumLtv", "O LTV máximo deve ser maior que zero.");

        return errors;
    }
}
