
using CreditRisk.Monitoring.Application.DTOs;
using CreditRisk.Monitoring.Domain.Enums;

namespace CreditRisk.Monitoring.Application.Validators;

public class MonitoringValidator
{
    public Dictionary<string, string> Validate(MonitoringRequest? request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados do monitoramento são obrigatórios.");
            return errors;
        }

        if (request.CreditOperationId == Guid.Empty)
            errors.Add(
                "CreditOperationId",
                "A operação de crédito é obrigatória.");

        if (request.RiskScore < 0 || request.RiskScore > 100)
            errors.Add(
                "RiskScore",
                "O score de risco deve estar entre 0 e 100.");

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

    public Dictionary<string, string> ValidateEvent(
        MonitoringEventRequest? request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados do evento são obrigatórios.");
            return errors;
        }

        if (request.CreditOperationId == Guid.Empty)
            errors.Add(
                "CreditOperationId",
                "A operação de crédito é obrigatória.");

        if (!Enum.IsDefined(request.Type))
            errors.Add(
                "Type",
                "O tipo de evento é inválido.");

        if (string.IsNullOrWhiteSpace(request.Description))
            errors.Add(
                "Description",
                "A descrição do evento é obrigatória.");

        return errors;
    }
}