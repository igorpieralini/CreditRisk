using System.Reflection;
using Xunit;

namespace CreditRisk.Tests.Guarantee.Entities;

public class GuaranteeEntityTests
{
    private static readonly Assembly GuaranteeAssembly =
        Assembly.Load("CreditRisk.Guarantee");

    [Fact]
    public void Guarantee_ShouldExistInDomain()
    {
        var entity = GetEntity("Guarantee");

        Assert.NotNull(entity);
        Assert.True(entity.IsClass);
        Assert.False(entity.IsAbstract);
    }

    [Fact]
    public void Guarantee_ShouldHaveIdentifier()
    {
        var entity = GetEntity("Guarantee");

        var property = entity.GetProperty(
            "Id",
            BindingFlags.Public | BindingFlags.Instance);

        Assert.NotNull(property);
        Assert.True(property.CanRead);
    }

    [Fact]
    public void GuaranteeValuation_ShouldExistInDomain()
    {
        var entity = GetEntity("GuaranteeValuation");

        Assert.NotNull(entity);
        Assert.True(entity.IsClass);
    }

    [Fact]
    public void GuaranteeOperation_ShouldExistInDomain()
    {
        var entity = GetEntity("GuaranteeOperation");

        Assert.NotNull(entity);
        Assert.True(entity.IsClass);
    }

    [Theory]
    [InlineData("Guarantee")]
    [InlineData("GuaranteeValuation")]
    [InlineData("GuaranteeOperation")]
    public void Entity_ShouldBelongToDomainNamespace(string entityName)
    {
        var entity = GetEntity(entityName);

        Assert.Contains(".Domain.", entity.Namespace);
    }

    private static Type GetEntity(string entityName)
    {
        var entity = GuaranteeAssembly
            .GetTypes()
            .FirstOrDefault(type =>
                type.IsClass &&
                type.Name == entityName);

        return Assert.IsType<Type>(entity, exactMatch: false);
    }
}