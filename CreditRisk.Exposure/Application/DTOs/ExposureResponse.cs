using Exposure.Domain.Enums;

namespace Exposure.Application.DTOs;

public class ExposureResponse
{
    public Guid Id { get; set; }
    public Guid CreditOperationId { get; set; }
    public Guid CreditLimitId { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public ExposureStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}