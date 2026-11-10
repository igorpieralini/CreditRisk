using CreditRisk.Guarantee.Domain.Enums;

namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeRequest
{
    public GuaranteeType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal OriginalValue { get; set; }
}