using CreditRisk.Monitoring.Domain.Entities;
using CreditRisk.Monitoring.Domain.Enums;
using Xunit;

namespace CreditRisk.Tests.Monitoring.Entities;

public class RiskAlertTests
{
    [Fact]
    public void RiskAlert_ShouldBeConcreteEntity()
    {
        var type = typeof(RiskAlert);

        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
    }

    [Fact]
    public void RiskAlert_ShouldHaveAlertLevelProperty()
    {
        var properties = typeof(RiskAlert).GetProperties();

        Assert.Contains(properties, property =>
            property.PropertyType == typeof(AlertLevel) ||
            property.PropertyType == typeof(AlertLevel?));
    }

    [Fact]
    public void RiskAlert_ShouldHaveAlertStatusProperty()
    {
        var properties = typeof(RiskAlert).GetProperties();

        Assert.Contains(properties, property =>
            property.PropertyType == typeof(AlertStatus) ||
            property.PropertyType == typeof(AlertStatus?));
    }

    [Theory]
    [InlineData(AlertLevel.Low, 1)]
    [InlineData(AlertLevel.Medium, 2)]
    [InlineData(AlertLevel.High, 3)]
    public void AlertLevel_ShouldHaveExpectedValue(
        AlertLevel level,
        int expected)
    {
        Assert.Equal(expected, (int)level);
    }

    [Theory]
    [InlineData(AlertStatus.Open, 1)]
    [InlineData(AlertStatus.Acknowledged, 2)]
    [InlineData(AlertStatus.Resolved, 3)]
    public void AlertStatus_ShouldHaveExpectedValue(
        AlertStatus status,
        int expected)
    {
        Assert.Equal(expected, (int)status);
    }

    [Fact]
    public void AlertStatus_ShouldHaveThreePossibleValues()
    {
        var values = Enum.GetValues<AlertStatus>();

        Assert.Equal(3, values.Length);
    }
}