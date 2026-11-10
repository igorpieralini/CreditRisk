using Exposure.Domain.Rules;
using Xunit;

namespace CreditRisk.Tests.Exposure.Rules;

public class CreditLimitRuleTests
{
    private readonly CreditLimitRule _rule = new();

    [Fact]
    public void Validate_ShouldReturnTrue_WhenExposureIsWithinLimit()
    {
        var result = _rule.Validate(50000m, 100000m);

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenExposureExceedsLimit()
    {
        var result = _rule.Validate(120000m, 100000m);

        Assert.False(result);
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenExposureEqualsLimit()
    {
        var result = _rule.Validate(100000m, 100000m);

        Assert.True(result);
    }

    [Fact]
    public void CanIncrease_ShouldReturnTrue_WhenLimitIsAvailable()
    {
        var result = _rule.CanIncrease(
            40000m,
            10000m,
            100000m);

        Assert.True(result);
    }

    [Fact]
    public void CanIncrease_ShouldReturnFalse_WhenLimitIsExceeded()
    {
        var result = _rule.CanIncrease(
            90000m,
            20000m,
            100000m);

        Assert.False(result);
    }

    [Fact]
    public void CanIncrease_ShouldReturnTrue_WhenIncreaseReachesExactLimit()
    {
        var result = _rule.CanIncrease(
            80000m,
            20000m,
            100000m);

        Assert.True(result);
    }

    [Fact]
    public void CanIncrease_ShouldThrowException_WhenAmountIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.CanIncrease(40000m, -10000m, 100000m));
    }

    [Fact]
    public void CanIncrease_ShouldThrowException_WhenLimitIsZero()
    {
        Assert.Throws<ArgumentException>(() =>
            _rule.CanIncrease(40000m, 10000m, 0m));
    }
}