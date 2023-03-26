namespace DependencyInjectionExtensions.Tests;

[ServiceLifetime(ServiceLifetime.Transient)]
public class TransientTestClass1 { }

[ServiceLifetime(ServiceLifetime.Transient)]
public class TransientTestClass2 { }

[ServiceLifetime(ServiceLifetime.Singleton)]
public class SingletonTestClass1 { }

[ServiceLifetime(ServiceLifetime.Singleton)]
public class SingletonTestClass2 { }

[ServiceLifetime(ServiceLifetime.Transient)]
[RegistrationTarget(typeof(TestInterface1))]
public class TestImplementationClass1 : TestInterface1 { }

public interface TestInterface1 { }
