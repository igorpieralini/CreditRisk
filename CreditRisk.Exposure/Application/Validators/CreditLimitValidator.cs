using Exposure.Application.DTOs;

namespace Exposure.Application.Validators;

public class CreditLimitValidator
{
    public Dictionary<string, string> Validate(CreditLimitRequest request)
    {
        var errors = new Dictionary<string, string>();

        if (request is null)
        {
            errors.Add("Request", "Os dados do limite de crédito são obrigatórios.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("Name", "O nome do limite de crédito é obrigatório.");
        else if (request.Name.Trim().Length > 180)
            errors.Add("Name", "O nome não pode ultrapassar 180 caracteres.");

        if (request.TotalAmount <= 0)
            errors.Add("TotalAmount", "O valor do limite deve ser maior que zero.");

        return errors;
    }
}