
using System.Reflection;
using Xunit;

namespace CreditRisk.Tests.Guarantee.Services;

public class GuaranteeServiceTests
{
    private static readonly Assembly GuaranteeAssembly =
        Assembly.Load("CreditRisk.Guarantee");

    [Fact]
    public void GuaranteeService_ShouldExist()
    {
        var service = GetService();

        Assert.True(service.IsClass);
        Assert.False(service.IsAbstract);
    }

    [Fact]
    public void GuaranteeService_ShouldHavePublicConstructor()
    {
        var constructors = GetService()
            .GetConstructors(
                BindingFlags.Public |
                BindingFlags.Instance);

        Assert.NotEmpty(constructors);
    }

    [Fact]
    public void GuaranteeService_ShouldExposeAsynchronousOperations()
    {
        var methods = GetPublicMethods()
            .Where(method => method.Name.EndsWith("Async"))
            .ToList();

        Assert.NotEmpty(methods);

        Assert.All(methods, method =>
            Assert.True(
                typeof(Task).IsAssignableFrom(method.ReturnType)));
    }

    [Fact]
    public void GuaranteeService_ShouldExposeCreationOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name == "CreateAsync");
    }

    [Fact]
    public void GuaranteeService_ShouldExposeGetByIdOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name == "GetByIdAsync");
    }

    [Fact]
    public void GuaranteeService_ShouldExposeGetAllOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name == "GetAllAsync");
    }

    [Fact]
    public void GuaranteeService_ShouldExposeDeleteOperation()
    {
        var methods = GetPublicMethods();

        Assert.Contains(methods, method =>
            method.Name == "DeleteAsync");
    }

    private static Type GetService()
    {
        return GuaranteeAssembly
            .GetTypes()
            .Single(type =>
                type.IsClass &&
                type.Name == "GuaranteeService");
    }

    private static MethodInfo[] GetPublicMethods()
    {
        return GetService()
            .GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);
    }
}
