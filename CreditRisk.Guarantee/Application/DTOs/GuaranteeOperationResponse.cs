namespace CreditRisk.Guarantee.Application.DTOs;

public class GuaranteeOperationResponse
{
    public Guid Id { get; set; }
    public Guid GuaranteeId { get; set; }
    public Guid CreditOperationId { get; set; }
    public decimal AllocatedValue { get; set; }
    public DateTime LinkedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool IsActive { get; set; }
}