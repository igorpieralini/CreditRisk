
using CreditRisk.RiskDecision.Domain.Calculations;
using Xunit;

namespace CreditRisk.Tests.RiskDecision.Calculations;

public class RiskScoreCalculatorTests
{
    private readonly RiskScoreCalculator _calculator = new();

    [Fact]
    public void Calculate_ShouldReturn100_WhenAllIndicatorsAreIdeal()
    {
        var result = _calculator.Calculate(100m, 100m, 0m);

        Assert.Equal(100m, result);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenAllIndicatorsAreWorst()
    {
        var result = _calculator.Calculate(0m, 0m, 100m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ShouldApplyCorrectWeights()
    {
        var result = _calculator.Calculate(80m, 90m, 50m);

        Assert.Equal(77m, result);
    }

    [Fact]
    public void Calculate_ShouldReturnExpectedScore_ForModerateRisk()
    {
        var result = _calculator.Calculate(60m, 80m, 80m);

        Assert.Equal(58m, result);
    }

    [Fact]
    public void Calculate_ShouldLimitGuaranteeCoverageTo100Percent()
    {
        var result = _calculator.Calculate(80m, 150m, 50m);

        Assert.Equal(80m, result);
    }

    [Fact]
    public void Calculate_ShouldLimitExposureUtilizationTo100Percent()
    {
        var result = _calculator.Calculate(80m, 100m, 120m);

        Assert.Equal(70m, result);
    }

    [Fact]
    public void Calculate_ShouldRoundResultToTwoDecimalPlaces()
    {
        var result = _calculator.Calculate(
            73.333m,
            86.666m,
            33.333m);

        Assert.Equal(76.00m, result);
    }

    [Theory]
    [InlineData(100, 100, 0, 100)]
    [InlineData(80, 90, 50, 77)]
    [InlineData(60, 80, 80, 58)]
    [InlineData(0, 0, 100, 0)]
    public void Calculate_ShouldReturnExpectedResult(
        int creditScore,
        int guaranteeCoverage,
        int limitUtilization,
        int expected)
    {
        var result = _calculator.Calculate(
            creditScore,
            guaranteeCoverage,
            limitUtilization);

        Assert.Equal((decimal)expected, result);
    }
}
