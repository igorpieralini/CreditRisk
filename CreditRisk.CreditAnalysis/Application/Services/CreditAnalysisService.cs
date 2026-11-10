
using CreditRisk.CreditAnalysis.Application.DTOs;
using CreditRisk.CreditAnalysis.Domain.Calculations;
using CreditRisk.CreditAnalysis.Domain.Models;
using CreditRisk.CreditAnalysis.Domain.Rules;

namespace CreditRisk.CreditAnalysis.Application.Services;

public class CreditAnalysisService : ICreditAnalysisService
{
    private readonly InterestCalculator _interestCalculator;
    private readonly InstallmentCalculator _installmentCalculator;
    private readonly TotalCostCalculator _totalCostCalculator;
    private readonly CreditLimitRule _creditLimitRule;
    private readonly PaymentTermRule _paymentTermRule;

    public CreditAnalysisService(
        InterestCalculator interestCalculator,
        InstallmentCalculator installmentCalculator,
        TotalCostCalculator totalCostCalculator,
        CreditLimitRule creditLimitRule,
        PaymentTermRule paymentTermRule)
    {
        _interestCalculator = interestCalculator;
        _installmentCalculator = installmentCalculator;
        _totalCostCalculator = totalCostCalculator;
        _creditLimitRule = creditLimitRule;
        _paymentTermRule = paymentTermRule;
    }

    public CreditAnalysisResponse Analyze(CreditAnalysisRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var operation = new CreditOperation(
            request.Amount,
            request.AnnualInterestRate,
            request.PaymentTermMonths
        );

        decimal monthlyRate = _interestCalculator.CalculateMonthlyRate(
            operation.AnnualInterestRate
        );

        decimal installment = _installmentCalculator.CalculateInstallment(
            operation.Amount,
            monthlyRate,
            operation.PaymentTermMonths
        );

        decimal totalPayment = _totalCostCalculator.CalculateTotalPayment(
            installment,
            operation.PaymentTermMonths
        );

        decimal totalInterest = _totalCostCalculator.CalculateTotalInterest(
            totalPayment,
            operation.Amount
        );

        bool withinCreditLimit = _creditLimitRule.Validate(
            operation.Amount,
            request.AvailableLimit
        );

        bool withinPaymentTerm = _paymentTermRule.Validate(
            operation.PaymentTermMonths,
            request.MaximumTermMonths
        );

        var result = new CreditAnalysisResult(
            operation.Id,
            monthlyRate,
            installment,
            totalPayment,
            totalInterest
        );

        return new CreditAnalysisResponse
        {
            OperationId = result.OperationId,
            MonthlyInterestRate = result.MonthlyInterestRate,
            InstallmentAmount = result.InstallmentAmount,
            TotalPayment = result.TotalPayment,
            TotalInterest = result.TotalInterest,
            IsWithinCreditLimit = withinCreditLimit,
            IsWithinPaymentTerm = withinPaymentTerm
        };
    }
}
