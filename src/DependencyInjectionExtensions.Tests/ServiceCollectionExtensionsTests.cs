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

    [Fact]
    public void RegisterServicesInAssembly_Should_Register_Implementation()
    {
        // arrange
        ServiceCollection sut = new ServiceCollection();

        // act
        sut.RegisterServicesInAssembly(typeof(TestImplementationClass1));
        var provider = sut.BuildServiceProvider();

        var implementation1 = provider.GetService<TestInterface1>();

        // assert
        implementation1.Should().NotBeNull().And.BeOfType<TestImplementationClass1>();
    }

    [Fact]
    public void RegisterServicesInAssembly_Should_Register_Multiple_Implementations()
    {
        // arrange
        ServiceCollection sut = new ServiceCollection();

        // act
        sut.RegisterServicesInAssembly(typeof(TestImplementationClass2));
        var provider = sut.BuildServiceProvider();

        var implementation2 = provider.GetService<TestInterface2>();
        var implementation3 = provider.GetService<TestInterface3>();

        // assert
        implementation2.Should().NotBeNull().And.BeOfType<TestImplementationClass2>();
        implementation3.Should().NotBeNull().And.BeOfType<TestImplementationClass2>();
    }
}


