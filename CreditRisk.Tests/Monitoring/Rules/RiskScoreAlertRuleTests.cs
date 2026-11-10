using CreditRisk.Monitoring.Domain.Enums;
using CreditRisk.Monitoring.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.Monitoring.Rules;

public class RiskScoreAlertRuleTests
{
    private readonly RiskScoreAlertRule _rule = new();

    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    [InlineData(39.99)]
    public void Evaluate_ShouldReturnHigh_WhenScoreIsBelow40(decimal score)
    {
        var result = _rule.Evaluate(score);

        Assert.Equal(AlertLevel.High, result);
    }

    [Theory]
    [InlineData(40)]
    [InlineData(50)]
    [InlineData(59.99)]
    public void Evaluate_ShouldReturnMedium_WhenScoreIsBetween40And60(decimal score)
    {
        var result = _rule.Evaluate(score);

        Assert.Equal(AlertLevel.Medium, result);
    }

    [Theory]
    [InlineData(60)]
    [InlineData(70)]
    [InlineData(74.99)]
    public void Evaluate_ShouldReturnLow_WhenScoreIsBetween60And75(decimal score)
    {
        var result = _rule.Evaluate(score);

        Assert.Equal(AlertLevel.Low, result);
    }

    [Theory]
    [InlineData(75)]
    [InlineData(90)]
    [InlineData(100)]
    public void Evaluate_ShouldReturnNull_WhenScoreIs75OrHigher(decimal score)
    {
        var result = _rule.Evaluate(score);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100.01)]
    [InlineData(150)]
    public void Evaluate_ShouldThrowException_WhenScoreIsOutsideValidRange(decimal score)
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Evaluate(score));
    }
}