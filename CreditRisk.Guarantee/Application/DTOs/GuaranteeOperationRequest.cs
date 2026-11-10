namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeOperationRequest
{
    public Guid GuaranteeId { get; set; }
    public Guid CreditOperationId { get; set; }
    public decimal AllocatedValue { get; set; }
}