using CreditRisk.RiskDecision.Application.DTOs;

namespace CreditRisk.RiskDecision.Application.Validators;

public class RiskDecisionValidator
{
    public Dictionary<string, string> Validate(RiskDecisionRequest? request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados da operação são obrigatórios.");
            return errors;
        }

        if (request.CreditOperationId == Guid.Empty)
            errors.Add(
                "CreditOperationId",
                "A operação de crédito é obrigatória.");

        if (request.RequestedAmount <= 0)
            errors.Add(
                "RequestedAmount",
                "O valor solicitado deve ser maior que zero.");

        if (request.CreditScore < 0 || request.CreditScore > 100)
            errors.Add(
                "CreditScore",
                "O score de crédito deve estar entre 0 e 100.");

        if (request.GuaranteeCoveragePercentage < 0)
            errors.Add(
                "GuaranteeCoveragePercentage",
                "A cobertura de garantias não pode ser negativa.");

        if (request.LimitUtilizationPercentage < 0)
            errors.Add(
                "LimitUtilizationPercentage",
                "A utilização do limite não pode ser negativa.");

        return errors;
    }
}