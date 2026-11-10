
using Exposure.Domain.Calculations;
using Xunit;

namespace CreditRisk.Tests.Exposure.Calculations;

public class ExposureCalculatorTests
{
    private readonly ExposureCalculator _calculator = new();

    [Fact]
    public void CalculateTotalExposure_ShouldReturnSum_WhenAmountsAreValid()
    {
        var amounts = new decimal[]
        {
            25000m,
            15000m,
            10000m
        };

        var result = _calculator.CalculateTotalExposure(amounts);

        Assert.Equal(50000m, result);
    }

    [Fact]
    public void CalculateTotalExposure_ShouldReturnZero_WhenCollectionIsEmpty()
    {
        var result = _calculator.CalculateTotalExposure(
            Array.Empty<decimal>());

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateTotalExposure_ShouldThrowException_WhenAmountIsNegative()
    {
        var amounts = new decimal[]
        {
            10000m,
            -5000m
        };

        Assert.Throws<ArgumentException>(() =>
            _calculator.CalculateTotalExposure(amounts));
    }

    [Fact]
    public void CalculateTotalExposure_ShouldThrowException_WhenCollectionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _calculator.CalculateTotalExposure(null!));
    }

    [Fact]
    public void CalculateAvailableLimit_ShouldReturnRemainingAmount()
    {
        var result = _calculator.CalculateAvailableLimit(
            100000m,
            40000m);

        Assert.Equal(60000m, result);
    }

    [Fact]
    public void CalculateAvailableLimit_ShouldReturnZero_WhenLimitIsFullyUsed()
    {
        var result = _calculator.CalculateAvailableLimit(
            100000m,
            100000m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateAvailableLimit_ShouldReturnZero_WhenExposureExceedsLimit()
    {
        var result = _calculator.CalculateAvailableLimit(
            100000m,
            120000m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateAvailableLimit_ShouldThrowException_WhenLimitIsZero()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.CalculateAvailableLimit(0m, 10000m));
    }

    [Fact]
    public void CalculateAvailableLimit_ShouldThrowException_WhenExposureIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            _calculator.CalculateAvailableLimit(100000m, -5000m));
    }
}
