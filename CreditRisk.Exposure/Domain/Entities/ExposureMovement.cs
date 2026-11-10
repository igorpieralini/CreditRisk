using Exposure.Domain.Enums;

namespace Exposure.Domain.Entities;

public class ExposureMovement
{
    public Guid Id { get; private set; }
    public Guid CreditExposureId { get; private set; }
    public ExposureMovementType Type { get; private set; }
    public decimal Amount { get; private set; }
    public decimal PreviousBalance { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public CreditExposure CreditExposure { get; private set; } = null!;

    private ExposureMovement()
    {
    }

    public ExposureMovement(
        Guid creditExposureId,
        ExposureMovementType type,
        decimal amount,
        decimal previousBalance,
        decimal currentBalance)
    {
        if (creditExposureId == Guid.Empty)
            throw new ArgumentException("A exposição de crédito é obrigatória.");

        if (!Enum.IsDefined(type))
            throw new ArgumentException("O tipo de movimentação é inválido.");

        if (amount <= 0)
            throw new ArgumentException("O valor da movimentação deve ser maior que zero.");

        if (previousBalance < 0 || currentBalance < 0)
            throw new ArgumentException("Os saldos não podem ser negativos.");

        if (type == ExposureMovementType.Increase &&
            currentBalance != previousBalance + amount)
            throw new ArgumentException("Saldo inválido para aumento de exposição.");

        if (type == ExposureMovementType.Reduction &&
            currentBalance != previousBalance - amount)
            throw new ArgumentException("Saldo inválido para redução de exposição.");

        Id = Guid.NewGuid();
        CreditExposureId = creditExposureId;
        Type = type;
        Amount = amount;
        PreviousBalance = previousBalance;
        CurrentBalance = currentBalance;
        CreatedAt = DateTime.UtcNow;
    }
}