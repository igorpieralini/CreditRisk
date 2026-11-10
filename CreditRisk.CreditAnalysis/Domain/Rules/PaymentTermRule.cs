namespace CreditRisk.CreditAnalysis.Domain.Rules;

public class PaymentTermRule
{
    public bool Validate(int paymentTermMonths, int maximumTermMonths)
    {
        if (paymentTermMonths <= 0)
            throw new ArgumentException("O prazo da operação deve ser maior que zero.");

        if (maximumTermMonths <= 0)
            throw new ArgumentException("O prazo máximo permitido deve ser maior que zero.");

        return paymentTermMonths <= maximumTermMonths;
    }
}