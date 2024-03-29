﻿namespace OsborneSupremacy.Extensions.Net.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegistrationTargetAttribute : Attribute
{
    public RegistrationTargetAttribute(Type type)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public Type Type { get; }
}

