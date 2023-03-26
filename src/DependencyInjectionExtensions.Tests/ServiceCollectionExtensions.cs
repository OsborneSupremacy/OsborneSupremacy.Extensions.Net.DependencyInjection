namespace DependencyInjectionExtensions.Tests;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static ServiceProvider BuildServiceProvider(this ServiceCollection input) =>
        input.BuildServiceProvider
        (
            new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true }
        );
}
