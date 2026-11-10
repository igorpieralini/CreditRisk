namespace CreditRisk.Guarantee.Domain.Entities;

public class GuaranteeOperation
{
    public Guid Id { get; private set; }
    public Guid GuaranteeId { get; private set; }
    public Guid CreditOperationId { get; private set; }
    public decimal AllocatedValue { get; private set; }
    public DateTime LinkedAt { get; private set; }
    public DateTime? ReleasedAt { get; private set; }
    public bool IsActive { get; private set; }

    public Guarantee Guarantee { get; private set; } = null!;

    private GuaranteeOperation()
    {
    }

    public GuaranteeOperation(
        Guid guaranteeId,
        Guid creditOperationId,
        decimal allocatedValue)
    {
        if (guaranteeId == Guid.Empty)
            throw new ArgumentException("A garantia é obrigatória.");

        if (creditOperationId == Guid.Empty)
            throw new ArgumentException("A operação de crédito é obrigatória.");

        if (allocatedValue <= 0)
            throw new ArgumentException("O valor vinculado deve ser maior que zero.");

        Id = Guid.NewGuid();
        GuaranteeId = guaranteeId;
        CreditOperationId = creditOperationId;
        AllocatedValue = allocatedValue;
        LinkedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Release()
    {
        if (!IsActive)
            throw new InvalidOperationException("A garantia já foi liberada.");

        IsActive = false;
        ReleasedAt = DateTime.UtcNow;
    }
}