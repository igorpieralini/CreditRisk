using Exposure.Domain.Calculations;
using Xunit;

namespace CreditRisk.Tests.Exposure.Calculations;

public class ConcentrationCalculatorTests
{
    private readonly ConcentrationCalculator _calculator = new();

    [Fact]
    public void Calculate_ShouldReturn50Percent_WhenCategoryRepresentsHalfOfTotal()
    {
        var result = _calculator.Calculate(50000m, 100000m);

        Assert.Equal(50m, result);
    }

    [Fact]
    public void Calculate_ShouldReturn100Percent_WhenCategoryEqualsTotal()
    {
        var result = _calculator.Calculate(100000m, 100000m);

        Assert.Equal(100m, result);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenCategoryExposureIsZero()
    {
        var result = _calculator.Calculate(0m, 100000m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenTotalExposureIsZero()
    {
        var result = _calculator.Calculate(0m, 0m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ShouldRoundToTwoDecimalPlaces()
    {
        var result = _calculator.Calculate(33333m, 100000m);

        Assert.Equal(33.33m, result);
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenCategoryExceedsTotal()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.Calculate(120000m, 100000m));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenCategoryIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.Calculate(-10000m, 100000m));
    }

    [Fact]
    public void Calculate_ShouldThrowException_WhenTotalIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.Calculate(10000m, -100000m));
    }
}