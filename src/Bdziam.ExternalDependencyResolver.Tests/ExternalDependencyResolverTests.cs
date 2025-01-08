using System;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace Bdziam.ExternalDependencyResolver.Tests;

public class ExternalDependencyResolverTests
{
    [Fact]
    public void Resolve_ShouldResolveTypeUsingServiceProvider()
    {
        // Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IDependency)))
            .Returns(new Dependency());

        var resolver = new ExternalDependencyResolver(serviceProviderMock.Object);

        // Act
        var result = resolver.Resolve<TestClass>();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TestClass>(result);
    }

    [Fact]
    public void Resolve_ShouldThrowExceptionWhenDependencyCannotBeResolved()
    {
        // Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();
        var resolver = new ExternalDependencyResolver(serviceProviderMock.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestClass>());
    }

    [Fact]
    public void Resolve_ShouldUseAdditionalDependenciesWhenOverrideIsTrue()
    {
        // Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();
        IDependency additionalDependency = new Dependency();
        var resolver = new ExternalDependencyResolver(serviceProviderMock.Object);

        // Act
        var result = resolver.Resolve<TestClass>(true, new KeyValuePair<Type, object>(typeof(IDependency), additionalDependency));

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TestClass>(result);
        Assert.Equal(additionalDependency, result.Dependency);
    }

    private class TestClass
    {
        public IDependency Dependency { get; }

        public TestClass(IDependency dependency)
        {
            Dependency = dependency;
        }
    }

    private interface IDependency { }

    private class Dependency : IDependency { }
}