using CreditRisk.Monitoring.Domain.Enums;
using CreditRisk.Monitoring.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.Monitoring.Rules;

public class GuaranteeAlertRuleTests
{
    private readonly GuaranteeAlertRule _rule = new();

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    [InlineData(49.99)]
    public void Evaluate_ShouldReturnHigh_WhenCoverageIsBelow50(decimal coverage)
    {
        var result = _rule.Evaluate(coverage);

        Assert.Equal(AlertLevel.High, result);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(65)]
    [InlineData(79.99)]
    public void Evaluate_ShouldReturnMedium_WhenCoverageIsBetween50And80(decimal coverage)
    {
        var result = _rule.Evaluate(coverage);

        Assert.Equal(AlertLevel.Medium, result);
    }

    [Theory]
    [InlineData(80)]
    [InlineData(90)]
    [InlineData(99.99)]
    public void Evaluate_ShouldReturnLow_WhenCoverageIsBetween80And100(decimal coverage)
    {
        var result = _rule.Evaluate(coverage);

        Assert.Equal(AlertLevel.Low, result);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(120)]
    [InlineData(150)]
    public void Evaluate_ShouldReturnNull_WhenCoverageIs100OrHigher(decimal coverage)
    {
        var result = _rule.Evaluate(coverage);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_ShouldThrowException_WhenCoverageIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Evaluate(-1m));
    }
}