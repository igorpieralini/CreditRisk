namespace Exposure.Application.DTOs;

public class CreditLimitRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}