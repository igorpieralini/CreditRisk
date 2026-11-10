namespace CreditRisk.Guarantee.Domain.Entities;

public class GuaranteeValuation
{
    public Guid Id { get; private set; }
    public Guid GuaranteeId { get; private set; }
    public decimal MarketValue { get; private set; }
    public decimal HaircutPercentage { get; private set; }
    public decimal EligibleValue { get; private set; }
    public DateTime ValuationDate { get; private set; }

    public Guarantee Guarantee { get; private set; } = null!;

    private GuaranteeValuation()
    {
    }

    public GuaranteeValuation(
        Guid guaranteeId,
        decimal marketValue,
        decimal haircutPercentage)
    {
        if (guaranteeId == Guid.Empty)
            throw new ArgumentException("A garantia é obrigatória.");

        if (marketValue <= 0)
            throw new ArgumentException("O valor de mercado deve ser maior que zero.");

        if (haircutPercentage < 0 || haircutPercentage > 100)
            throw new ArgumentException("O haircut deve estar entre 0 e 100%.");

        Id = Guid.NewGuid();
        GuaranteeId = guaranteeId;
        MarketValue = marketValue;
        HaircutPercentage = haircutPercentage;
        EligibleValue = Math.Round(
            marketValue * (1 - haircutPercentage / 100m), 2
        );
        ValuationDate = DateTime.UtcNow;
    }
}