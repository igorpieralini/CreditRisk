using CreditRisk.Monitoring.Domain.Enums;
using CreditRisk.Monitoring.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.Monitoring.Rules;

public class ExposureAlertRuleTests
{
    private readonly ExposureAlertRule _rule = new();

    [Theory]
    [InlineData(100)]
    [InlineData(110)]
    [InlineData(150)]
    public void Evaluate_ShouldReturnHigh_WhenUtilizationIs100OrHigher(decimal utilization)
    {
        var result = _rule.Evaluate(utilization);

        Assert.Equal(AlertLevel.High, result);
    }

    [Theory]
    [InlineData(90)]
    [InlineData(95)]
    [InlineData(99.99)]
    public void Evaluate_ShouldReturnMedium_WhenUtilizationIsBetween90And100(decimal utilization)
    {
        var result = _rule.Evaluate(utilization);

        Assert.Equal(AlertLevel.Medium, result);
    }

    [Theory]
    [InlineData(80)]
    [InlineData(85)]
    [InlineData(89.99)]
    public void Evaluate_ShouldReturnLow_WhenUtilizationIsBetween80And90(decimal utilization)
    {
        var result = _rule.Evaluate(utilization);

        Assert.Equal(AlertLevel.Low, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(79.99)]
    public void Evaluate_ShouldReturnNull_WhenUtilizationIsBelow80(decimal utilization)
    {
        var result = _rule.Evaluate(utilization);

        Assert.Null(result);
    }

    [Fact]
    public void Evaluate_ShouldThrowException_WhenUtilizationIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Evaluate(-1m));
    }
}