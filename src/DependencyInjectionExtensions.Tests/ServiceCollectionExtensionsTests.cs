using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OsborneSupremacy.Extensions.Net.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DependencyInjectionExtensions.Tests;

[ExcludeFromCodeCoverage]
public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterServicesInAssembly_Should_Register_Transient_Classes()
    {
        // arrange
        ServiceCollection sut = new ServiceCollection();

        // act
        sut.RegisterServicesInAssembly(typeof(TransientTestClass1));
        var provider = sut.BuildServiceProvider();

        var class1i1 = provider.GetService(typeof(TransientTestClass1));
        var class2i1 = provider.GetService(typeof(TransientTestClass2));
        var class1i2 = provider.GetService(typeof(TransientTestClass1));

        // assert
        class1i1.Should().NotBeNull().And.NotBe(class1i2);
        class2i1.Should().NotBeNull();
    }

    [Fact]
    public void RegisterServicesInAssembly_Should_Register_Scoped_Classes()
    {
        // arrange
        ServiceCollection sut = new ServiceCollection();

        // act
        sut.RegisterServicesInAssembly(typeof(TransientTestClass1));
        var provider = sut.BuildServiceProvider();

        var class1i1 = provider.GetService(typeof(SingletonTestClass1));
        var class2i1 = provider.GetService(typeof(SingletonTestClass2));
        var class1i2 = provider.GetService(typeof(SingletonTestClass1));

        // assert
        class1i1.Should().NotBeNull().And.Be(class1i2);
        class2i1.Should().NotBeNull();
    }

}

public static class ServiceCollectionExtensions
{
    public static ServiceProvider BuildServiceProvider(this ServiceCollection input) =>
        input.BuildServiceProvider
        (
            new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true }
        );
}

[ServiceLifetime(ServiceLifetime.Transient)]
public class TransientTestClass1 { }

[ServiceLifetime(ServiceLifetime.Transient)]
public class TransientTestClass2 { }

[ServiceLifetime(ServiceLifetime.Singleton)]
public class SingletonTestClass1 { }

[ServiceLifetime(ServiceLifetime.Singleton)]
public class SingletonTestClass2 { }
