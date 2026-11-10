using System.Reflection;
using CreditRisk.RiskDecision.Application.Services;
using Xunit;

namespace CreditRisk.Tests.RiskDecision.Services;

public class RiskDecisionServiceTests
{
    [Fact]
    public void RiskDecisionService_ShouldBeConcreteClass()
    {
        var type = typeof(RiskDecisionService);

        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
    }

    [Fact]
    public void RiskDecisionService_ShouldExposeEvaluateAsync()
    {
        var methods = typeof(RiskDecisionService)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance);

        Assert.Contains(methods, method =>
            method.Name == "EvaluateAsync");
    }

    [Fact]
    public void EvaluateAsync_ShouldReturnTask()
    {
        var method = typeof(RiskDecisionService)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(method => method.Name == "EvaluateAsync");

        Assert.True(
            typeof(Task).IsAssignableFrom(method.ReturnType));
    }
}