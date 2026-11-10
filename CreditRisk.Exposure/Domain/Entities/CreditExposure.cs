using Exposure.Domain.Enums;

namespace Exposure.Domain.Entities;

public class CreditExposure
{
    public Guid Id { get; private set; }
    public Guid CreditOperationId { get; private set; }
    public Guid CreditLimitId { get; private set; }
    public decimal OriginalAmount { get; private set; }
    public decimal OutstandingAmount { get; private set; }
    public ExposureStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public CreditLimit CreditLimit { get; private set; } = null!;

    public ICollection<ExposureMovement> Movements { get; private set; }
        = new List<ExposureMovement>();

    private CreditExposure()
    {
    }

    public CreditExposure(
        Guid creditOperationId,
        Guid creditLimitId,
        decimal amount)
    {
        if (creditOperationId == Guid.Empty)
            throw new ArgumentException("A operação de crédito é obrigatória.");

        if (creditLimitId == Guid.Empty)
            throw new ArgumentException("O limite de crédito é obrigatório.");

        if (amount <= 0)
            throw new ArgumentException("O valor da exposição deve ser maior que zero.");

        Id = Guid.NewGuid();
        CreditOperationId = creditOperationId;
        CreditLimitId = creditLimitId;
        OriginalAmount = amount;
        OutstandingAmount = amount;
        Status = ExposureStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Increase(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor do aumento deve ser maior que zero.");

        if (Status != ExposureStatus.Active)
            throw new InvalidOperationException("A exposição não está ativa.");

        OutstandingAmount += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reduce(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da redução deve ser maior que zero.");

        if (Status != ExposureStatus.Active)
            throw new InvalidOperationException("A exposição não está ativa.");

        if (amount > OutstandingAmount)
            throw new InvalidOperationException(
                "A redução não pode ultrapassar o saldo da exposição.");

        OutstandingAmount -= amount;
        UpdatedAt = DateTime.UtcNow;

        if (OutstandingAmount == 0)
            Status = ExposureStatus.Settled;
    }
}