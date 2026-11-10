
using System.Reflection;
using Xunit;

namespace CreditRisk.Tests.CreditAnalysis.Services;

public class CreditAnalysisServiceTests
{
    private static readonly Assembly CreditAnalysisAssembly =
        Assembly.Load("CreditRisk.CreditAnalysis");

    [Fact]
    public void CreditAnalysis_ShouldContainApplicationServices()
    {
        var services = GetServices();

        Assert.NotEmpty(services);
    }

    [Fact]
    public void Services_ShouldBeConcreteClasses()
    {
        var services = GetServices();

        Assert.All(services, service =>
        {
            Assert.True(service.IsClass);
            Assert.False(service.IsAbstract);
        });
    }

    [Fact]
    public void Services_ShouldExposePublicOperations()
    {
        var services = GetServices();

        Assert.All(services, service =>
        {
            var methods = service.GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);

            Assert.NotEmpty(methods);
        });
    }

    [Fact]
    public void Services_ShouldNotDependDirectlyOnEntityFramework()
    {
        foreach (var service in GetServices())
        {
            var dependencies = service
                .GetConstructors()
                .SelectMany(constructor => constructor.GetParameters())
                .Select(parameter => parameter.ParameterType);

            Assert.DoesNotContain(dependencies, dependency =>
                dependency.FullName?.StartsWith(
                    "Microsoft.EntityFrameworkCore",
                    StringComparison.Ordinal) == true);
        }
    }

    private static List<Type> GetServices()
    {
        return CreditAnalysisAssembly
            .GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Name.EndsWith("Service", StringComparison.Ordinal) &&
                type.Namespace?.Contains(".Application.") == true)
            .ToList();
    }
}
