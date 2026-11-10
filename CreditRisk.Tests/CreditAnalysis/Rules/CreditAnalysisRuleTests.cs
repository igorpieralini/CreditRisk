
using System.Reflection;
using Xunit;

namespace CreditRisk.Tests.CreditAnalysis.Rules;

public class CreditAnalysisRuleTests
{
    private static readonly Assembly CreditAnalysisAssembly =
        Assembly.Load("CreditRisk.CreditAnalysis");

    [Fact]
    public void CreditAnalysisAssembly_ShouldLoadSuccessfully()
    {
        Assert.NotNull(CreditAnalysisAssembly);
    }

    [Fact]
    public void Rules_ShouldNotBeAbstract()
    {
        var rules = GetRules();

        Assert.All(rules, rule =>
            Assert.False(rule.IsAbstract));
    }

    [Fact]
    public void Rules_ShouldNotDependDirectlyOnEntityFramework()
    {
        var rules = GetRules();

        foreach (var rule in rules)
        {
            var dependencies = GetConstructorDependencies(rule);

            Assert.DoesNotContain(dependencies, dependency =>
                dependency.FullName?.StartsWith(
                    "Microsoft.EntityFrameworkCore",
                    StringComparison.Ordinal) == true);
        }
    }

    [Fact]
    public void Rules_ShouldNotDependDirectlyOnMassTransit()
    {
        var rules = GetRules();

        foreach (var rule in rules)
        {
            var dependencies = GetConstructorDependencies(rule);

            Assert.DoesNotContain(dependencies, dependency =>
                dependency.FullName?.StartsWith(
                    "MassTransit",
                    StringComparison.Ordinal) == true);
        }
    }

    [Fact]
    public void Rules_ShouldHavePublicMethods()
    {
        var rules = GetRules();

        foreach (var rule in rules)
        {
            var methods = rule.GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            Assert.NotEmpty(methods);
        }
    }

    private static List<Type> GetRules()
    {
        return CreditAnalysisAssembly
            .GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Name.EndsWith("Rule", StringComparison.Ordinal))
            .ToList();
    }

    private static IEnumerable<Type> GetConstructorDependencies(Type type)
    {
        return type
            .GetConstructors()
            .SelectMany(constructor =>
                constructor.GetParameters())
            .Select(parameter => parameter.ParameterType);
    }
}
