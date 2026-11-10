using CreditRisk.RiskDecision.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.RiskDecision.Rules;

public class ExposureLimitRuleTests
{
    private readonly ExposureLimitRule _rule = new();

    [Fact]
    public void Validate_ShouldReturnTrue_WhenUtilizationIsBelowMaximum()
    {
        var result = _rule.Validate(60m, 80m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenUtilizationEqualsMaximum()
    {
        var result = _rule.Validate(80m, 80m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenUtilizationExceedsMaximum()
    {
        var result = _rule.Validate(90m, 80m);

        Assert.False(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenUtilizationIsZero()
    {
        var result = _rule.Validate(0m, 80m);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 80, true)]
    [InlineData(50, 80, true)]
    [InlineData(79, 80, true)]
    [InlineData(80, 80, true)]
    [InlineData(81, 80, false)]
    [InlineData(100, 80, false)]
    public void Validate_ShouldReturnExpectedResult(
        int utilization,
        int maximumUtilization,
        bool expected)
    {
        var result = _rule.Validate(
            utilization,
            maximumUtilization);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenUtilizationIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(-10m, 80m));
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenMaximumUtilizationIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(50m, -10m));
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenMaximumUtilizationExceeds100()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(50m, 110m));
    }
}