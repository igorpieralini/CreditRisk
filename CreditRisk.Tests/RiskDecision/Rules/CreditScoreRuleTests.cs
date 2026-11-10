using CreditRisk.RiskDecision.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.RiskDecision.Rules;

public class CreditScoreRuleTests
{
    private readonly CreditScoreRule _rule = new();

    [Fact]
    public void Validate_ShouldReturnTrue_WhenScoreIsAboveMinimum()
    {
        var result = _rule.Validate(85m, 70m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenScoreEqualsMinimum()
    {
        var result = _rule.Validate(70m, 70m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenScoreIsBelowMinimum()
    {
        var result = _rule.Validate(60m, 70m);

        Assert.False(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenScoreIs100()
    {
        var result = _rule.Validate(100m, 70m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenScoreIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(-10m, 70m));
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenScoreExceeds100()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(110m, 70m));
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenMinimumScoreIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(80m, -10m));
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenMinimumScoreExceeds100()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.Validate(80m, 110m));
    }
}