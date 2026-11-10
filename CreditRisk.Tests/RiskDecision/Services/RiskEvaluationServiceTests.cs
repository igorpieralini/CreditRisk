using System.Reflection;
using CreditRisk.RiskDecision.Application.Services;
using Xunit;

namespace CreditRisk.Tests.RiskDecision.Services;

public class RiskEvaluationServiceTests
{
    [Fact]
    public void RiskEvaluationService_ShouldBeConcreteClass()
    {
        var type = typeof(RiskEvaluationService);

        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
    }

    [Fact]
    public void RiskEvaluationService_ShouldHavePublicConstructor()
    {
        var constructors = typeof(RiskEvaluationService)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        Assert.NotEmpty(constructors);
    }

    [Fact]
    public void RiskEvaluationService_ShouldExposeEvaluationOperation()
    {
        var methods = typeof(RiskEvaluationService)
            .GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);

        Assert.Contains(methods, method =>
            method.Name.Contains(
                "Evaluat",
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void RiskEvaluationService_ShouldNotDependDirectlyOnDbContext()
    {
        var dependencies = typeof(RiskEvaluationService)
            .GetConstructors()
            .SelectMany(constructor => constructor.GetParameters())
            .Select(parameter => parameter.ParameterType);

        Assert.DoesNotContain(dependencies, dependency =>
            dependency.Name.EndsWith("DbContext"));
    }
}