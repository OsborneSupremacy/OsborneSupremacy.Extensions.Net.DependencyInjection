namespace OsborneSupremacy.Extensions.Net.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServicesInAssembly(this IServiceCollection serviceCollection, Type type) =>
        serviceCollection
            .RegisterServicesInAssembly(type.Assembly);

    public static IServiceCollection RegisterServicesInAssemblyContaining<T>(this IServiceCollection serviceCollection) =>
        serviceCollection
            .RegisterServicesInAssembly(typeof(T).Assembly);

    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceCollection RegisterServicesInAssembly(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var services = assembly
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.IsPublic)
            .Where(x => x.GetCustomAttribute<ServiceLifetimeAttribute>() is not null);

        foreach (var service in services)
        {
            var lifetime = service.GetCustomAttribute<ServiceLifetimeAttribute>()!.ServiceLifetime;

            var targets = service.GetCustomAttributes<RegistrationTargetAttribute>().ToList();

            if(!targets.Any())
            {
                serviceCollection.AddService(service, lifetime);
                continue;
            }

            foreach(var target in targets)
                serviceCollection.AddService(target.Type, service, lifetime);
        }

        return serviceCollection;
    }

    private delegate IServiceCollection AddServiceDelegate(Type service, IServiceCollection serviceCollection);

    private static readonly AddServiceDelegate AddTransientDelegate =
        (service, serviceCollection) => serviceCollection.AddTransient(service);

    private static readonly AddServiceDelegate AddSingletonDelegate =
        (service, serviceCollection) => serviceCollection.AddSingleton(service);

    private static readonly AddServiceDelegate AddScopedDelegate =
        (service, serviceCollection) => serviceCollection.AddScoped(service);

    private static readonly Dictionary<ServiceLifetime, AddServiceDelegate>
        AddDelegateMap = new()
        {
            { ServiceLifetime.Transient, AddTransientDelegate },
            { ServiceLifetime.Singleton, AddSingletonDelegate },
            { ServiceLifetime.Scoped, AddScopedDelegate }
        };

    private delegate IServiceCollection AddImplementationDelegte(
        Type serviceType,
        Type implementationType,
        IServiceCollection serviceCollection
    );

    private static readonly AddImplementationDelegte AddTransientImplementationDelegate =
        (serviceType, implementationType, serviceCollection) => serviceCollection.AddTransient(serviceType, implementationType);

    private static readonly AddImplementationDelegte AddSingletonImplementationDelegate =
        (serviceType, implementationType, serviceCollection) => serviceCollection.AddSingleton(serviceType, implementationType);

    private static readonly AddImplementationDelegte AddScopedImplementationDelegate =
        (serviceType, implementationType, serviceCollection) => serviceCollection.AddScoped(serviceType, implementationType);

    private static readonly Dictionary<ServiceLifetime, AddImplementationDelegte>
        AddImplementationDelegateMap = new()
        {
            { ServiceLifetime.Transient, AddTransientImplementationDelegate },
            { ServiceLifetime.Singleton, AddSingletonImplementationDelegate },
            { ServiceLifetime.Scoped, AddScopedImplementationDelegate }
        };

    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceCollection AddService(
        this IServiceCollection serviceCollection,
        Type service,
        ServiceLifetime serviceLifetime
        )
    {
        if (!AddDelegateMap.TryGetValue(serviceLifetime, out var addDelegate))
            throw NotFoundExceptionDelegate(serviceLifetime);

        return addDelegate(service, serviceCollection);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceCollection AddService(
        this IServiceCollection serviceCollection,
        Type serviceType,
        Type implementationType,
        ServiceLifetime serviceLifetime
        )
    {
        if (!AddImplementationDelegateMap.TryGetValue(serviceLifetime, out var addDelegate))
            throw NotFoundExceptionDelegate(serviceLifetime);

        return addDelegate(serviceType, implementationType, serviceCollection);
    }

    private static readonly Func<ServiceLifetime, ArgumentOutOfRangeException> NotFoundExceptionDelegate = serviceLifetime =>
        new ArgumentOutOfRangeException($"{nameof(ServiceLifetime)} out of range: {serviceLifetime}");
}
