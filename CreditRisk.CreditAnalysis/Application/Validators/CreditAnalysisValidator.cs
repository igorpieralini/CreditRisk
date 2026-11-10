using CreditRisk.CreditAnalysis.Application.DTOs;

namespace CreditRisk.CreditAnalysis.Application.Validators;

public class CreditAnalysisValidator
{
    public Dictionary<string, string> Validate(CreditAnalysisRequest request)
    {
        var errors = new Dictionary<string, string>();

        if (request == null)
        {
            errors.Add("Request", "Os dados da operação são obrigatórios.");
            return errors;
        }

        if (request.Amount <= 0)
            errors.Add("Amount", "O valor da operação deve ser maior que zero.");

        if (request.AnnualInterestRate < 0)
            errors.Add("AnnualInterestRate", "A taxa anual não pode ser negativa.");

        if (request.PaymentTermMonths <= 0)
            errors.Add("PaymentTermMonths", "O prazo deve ser maior que zero.");

        if (request.AvailableLimit < 0)
            errors.Add("AvailableLimit", "O limite disponível não pode ser negativo.");

        if (request.MaximumTermMonths <= 0)
            errors.Add("MaximumTermMonths", "O prazo máximo deve ser maior que zero.");

        if (!Enum.IsDefined(request.CreditType))
            errors.Add("CreditType", "A modalidade de crédito é inválida.");

        return errors;
    }
}