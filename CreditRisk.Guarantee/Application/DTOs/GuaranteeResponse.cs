using CreditRisk.Guarantee.Domain.Enums;

namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeResponse
{
    public Guid Id { get; set; }
    public GuaranteeType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal OriginalValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}