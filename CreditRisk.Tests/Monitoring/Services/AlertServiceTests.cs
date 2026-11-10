using System.Reflection;
using CreditRisk.Monitoring.Application.Services;
using Xunit;

namespace CreditRisk.Tests.Monitoring.Services;

public class AlertServiceTests
{
    [Fact]
    public void AlertService_ShouldBeConcreteClass()
    {
        var type = typeof(AlertService);

        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
    }

    [Fact]
    public void AlertService_ShouldExposeAcknowledgementOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name.Contains(
                "Acknowledge",
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void AlertService_ShouldExposeResolutionOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name.Contains(
                "Resolve",
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void AlertService_ShouldUseAsynchronousOperations()
    {
        var methods = GetPublicMethods()
            .Where(method =>
                method.Name.Contains("Acknowledge") ||
                method.Name.Contains("Resolve"))
            .ToList();

        Assert.NotEmpty(methods);

        Assert.All(methods, method =>
            Assert.True(
                typeof(Task).IsAssignableFrom(method.ReturnType)));
    }

    [Fact]
    public void AlertService_ShouldSupportDependencyInjection()
    {
        var constructors = typeof(AlertService)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        Assert.NotEmpty(constructors);
    }

    private static MethodInfo[] GetPublicMethods()
    {
        return typeof(AlertService)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    }
}