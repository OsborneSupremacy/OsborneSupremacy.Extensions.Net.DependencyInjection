namespace OsborneSupremacy.Extensions.Net.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServicesInAssembly(this IServiceCollection serviceCollection, Type type)
    {
        var services = Assembly.GetAssembly(type)!
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsPublic);

        foreach (var service in services)
        {
            var lifetime = service.GetCustomAttribute<ServiceLifetimeAttribute>()?.ServiceLifetime;
            if (lifetime == null)
                continue;

            var targetType = service.GetCustomAttribute<RegistrationTargetAttribute>()?.Type;

            if (targetType == null)
                serviceCollection.AddService(service, lifetime.Value);
            else
                serviceCollection.AddService(targetType, service, lifetime.Value);
        }

        return serviceCollection;
    }

    private delegate IServiceCollection AddServiceDelegate(Type service, IServiceCollection serviceCollection);

    private static readonly AddServiceDelegate _addTransientDelegate =
        (Type service, IServiceCollection serviceCollection) =>
        {
            return serviceCollection.AddTransient(service);
        };

    private static readonly AddServiceDelegate _addSingletonDelegate =
        (Type service, IServiceCollection serviceCollection) =>
        {
            return serviceCollection.AddSingleton(service);
        };

    private static readonly AddServiceDelegate _addScopedDelegate =
        (Type service, IServiceCollection serviceCollection) =>
        {
            return serviceCollection.AddScoped(service);
        };

    private static readonly Dictionary<ServiceLifetime, AddServiceDelegate>
        _addDelegateMap = new()
        {
            { ServiceLifetime.Transient, _addTransientDelegate },
            { ServiceLifetime.Singleton, _addSingletonDelegate },
            { ServiceLifetime.Scoped, _addScopedDelegate }
        };

    private delegate IServiceCollection AddImplementationDelegte(
        Type serviceType,
        Type implementationType,
        IServiceCollection serviceCollection
    );

    private static readonly AddImplementationDelegte _addTransientImplementationDelegate =
        (Type serviceType, Type implementationType, IServiceCollection serviceCollection) =>
        {
            return serviceCollection.AddTransient(serviceType, implementationType);
        };

    private static readonly AddImplementationDelegte _addSingletonImplementationDelegate =
        (Type serviceType, Type implementationType, IServiceCollection serviceCollection) =>
        {
            return serviceCollection.AddSingleton(serviceType, implementationType);
        };

    private static readonly AddImplementationDelegte _addScopedImplementationDelegate =
        (Type serviceType, Type implementationType, IServiceCollection serviceCollection) =>
        {
            return serviceCollection.AddScoped(serviceType, implementationType);
        };

    private static readonly Dictionary<ServiceLifetime, AddImplementationDelegte>
        _addImplementationDelegateMap = new()
        {
            { ServiceLifetime.Transient, _addTransientImplementationDelegate },
            { ServiceLifetime.Singleton, _addSingletonImplementationDelegate },
            { ServiceLifetime.Scoped, _addScopedImplementationDelegate }
        };

    public static IServiceCollection AddService(
        this IServiceCollection serviceCollection,
        Type service,
        ServiceLifetime serviceLifetime
        )
    {
        if (!_addDelegateMap.TryGetValue(serviceLifetime, out var addDelegate))
            throw new ArgumentOutOfRangeException(serviceLifetime.ToString());

        return addDelegate(service, serviceCollection);
    }

    public static IServiceCollection AddService(
        this IServiceCollection serviceCollection,
        Type serviceType,
        Type implementationType,
        ServiceLifetime serviceLifetime
        )
    {
        if (!_addImplementationDelegateMap.TryGetValue(serviceLifetime, out var addDelegate))
            throw new ArgumentOutOfRangeException(serviceLifetime.ToString());

        return addDelegate(serviceType, implementationType, serviceCollection);
    }
}
