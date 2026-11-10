
using System.Reflection;
using CreditRisk.Monitoring.Application.Services;
using Xunit;

namespace CreditRisk.Tests.Monitoring.Services;

public class MonitoringServiceTests
{
    [Fact]
    public void MonitoringService_ShouldBeConcreteClass()
    {
        var type = typeof(MonitoringService);

        Assert.True(type.IsClass);
        Assert.False(type.IsAbstract);
    }

    [Fact]
    public void MonitoringService_ShouldExposeCreateOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name.Contains(
                "Create",
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void MonitoringService_ShouldExposeUpdateOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name.Contains(
                "Update",
                StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void MonitoringService_ShouldExposeRegisterEventAsync()
    {
        var method = GetPublicMethods()
            .FirstOrDefault(method =>
                method.Name == "RegisterEventAsync");

        Assert.NotNull(method);
        Assert.True(typeof(Task).IsAssignableFrom(method.ReturnType));
    }

    [Fact]
    public void MonitoringService_ShouldSupportDependencyInjection()
    {
        var constructors = typeof(MonitoringService)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        Assert.NotEmpty(constructors);
    }

    [Fact]
    public void MonitoringService_ShouldExposeAsynchronousOperations()
    {
        var methods = GetPublicMethods()
            .Where(method => method.Name.EndsWith("Async"))
            .ToList();

        Assert.NotEmpty(methods);

        Assert.All(methods, method =>
            Assert.True(
                typeof(Task).IsAssignableFrom(method.ReturnType)));
    }

    private static MethodInfo[] GetPublicMethods()
    {
        return typeof(MonitoringService)
            .GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);
    }
}
