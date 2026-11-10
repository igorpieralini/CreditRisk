namespace Exposure.Application.DTOs;

public class ExposureRequest
{
    public Guid CreditOperationId { get; set; }
    public Guid CreditLimitId { get; set; }
    public decimal Amount { get; set; }
}