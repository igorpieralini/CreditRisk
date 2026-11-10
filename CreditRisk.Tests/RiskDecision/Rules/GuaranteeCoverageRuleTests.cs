using CreditRisk.RiskDecision.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.RiskDecision.Rules;

public class GuaranteeCoverageRuleTests
{
    private readonly GuaranteeCoverageRule _rule = new();

    [Fact]
    public void Validate_ShouldReturnTrue_WhenCoverageExceedsMinimum()
    {
        var result = _rule.Validate(120m, 80m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenCoverageEqualsMinimum()
    {
        var result = _rule.Validate(80m, 80m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenCoverageIsBelowMinimum()
    {
        var result = _rule.Validate(50m, 80m);

        Assert.False(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenMinimumCoverageIsZero()
    {
        var result = _rule.Validate(0m, 0m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenCoverageExceeds100Percent()
    {
        var result = _rule.Validate(150m, 100m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenCoverageIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(-10m, 80m));
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenMinimumCoverageIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(100m, -20m));
    }

    [Theory]
    [InlineData(79, 80, false)]
    [InlineData(80, 80, true)]
    [InlineData(100, 80, true)]
    [InlineData(120, 100, true)]
    public void Validate_ShouldReturnExpectedResult(
        int coverage,
        int minimumCoverage,
        bool expected)
    {
        var result = _rule.Validate(
            coverage,
            minimumCoverage);

        Assert.Equal(expected, result);
    }
}