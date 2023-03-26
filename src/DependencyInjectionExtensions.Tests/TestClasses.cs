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

[ServiceLifetime(ServiceLifetime.Transient)]
[RegistrationTarget(typeof(TestInterface2))]
[RegistrationTarget(typeof(TestInterface3))]
public class TestImplementationClass2 : TestInterface2, TestInterface3 { }

public interface TestInterface1 { }

public interface TestInterface2 { }

public interface TestInterface3 { }
