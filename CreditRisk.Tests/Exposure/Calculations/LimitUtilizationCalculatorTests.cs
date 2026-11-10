using Exposure.Domain.Calculations;
using Xunit;

namespace CreditRisk.Tests.Exposure.Calculations;

public class LimitUtilizationCalculatorTests
{
    private readonly LimitUtilizationCalculator _calculator = new();

    [Fact]
    public void Calculate_ShouldReturn50Percent_WhenHalfOfLimitIsUsed()
    {
        var result = _calculator.Calculate(50000m, 100000m);

        Assert.Equal(50m, result);
    }

    [Fact]
    public void Calculate_ShouldReturn100Percent_WhenLimitIsFullyUsed()
    {
        var result = _calculator.Calculate(100000m, 100000m);

        Assert.Equal(100m, result);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenExposureIsZero()
    {
        var result = _calculator.Calculate(0m, 100000m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ShouldReturnAbove100Percent_WhenLimitIsExceeded()
    {
        var result = _calculator.Calculate(120000m, 100000m);

        Assert.Equal(120m, result);
    }

    [Fact]
    public void Calculate_ShouldRoundResultToTwoDecimalPlaces()
    {
        var result = _calculator.Calculate(33333m, 100000m);

        Assert.Equal(33.33m, result);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenLimitIsZero()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.Calculate(10000m, 0m));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenLimitIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.Calculate(10000m, -100000m));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenExposureIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.Calculate(-10000m, 100000m));
    }
}