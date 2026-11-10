namespace Exposure.Domain.Entities;

public class CreditLimit
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<CreditExposure> Exposures { get; private set; }
        = new List<CreditExposure>();

    private CreditLimit()
    {
    }

    public CreditLimit(string name, decimal totalAmount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do limite é obrigatório.");

        if (totalAmount <= 0)
            throw new ArgumentException("O limite deve ser maior que zero.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        TotalAmount = totalAmount;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateLimit(decimal totalAmount)
    {
        if (!IsActive)
            throw new InvalidOperationException("O limite está inativo.");

        if (totalAmount <= 0)
            throw new ArgumentException("O limite deve ser maior que zero.");

        TotalAmount = totalAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("O limite já está inativo.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("O limite já está ativo.");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}