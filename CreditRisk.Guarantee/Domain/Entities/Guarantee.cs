using CreditRisk.Guarantee.Domain.Enums;

namespace CreditRisk.Guarantee.Domain.Entities;

public class Guarantee
{
    public Guid Id { get; private set; }
    public GuaranteeType Type { get; private set; }
    public string Description { get; private set; }
    public decimal OriginalValue { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<GuaranteeValuation> Valuations { get; private set; }
    public ICollection<GuaranteeOperation> Operations { get; private set; }

    private Guarantee()
    {
        Description = string.Empty;
        Valuations = new List<GuaranteeValuation>();
        Operations = new List<GuaranteeOperation>();
    }

    public Guarantee(
        GuaranteeType type,
        string description,
        decimal originalValue)
        : this()
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentException("Tipo de garantia inválido.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição da garantia é obrigatória.");

        if (originalValue <= 0)
            throw new ArgumentException("O valor da garantia deve ser maior que zero.");

        Id = Guid.NewGuid();
        Type = type;
        Description = description.Trim();
        OriginalValue = originalValue;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}