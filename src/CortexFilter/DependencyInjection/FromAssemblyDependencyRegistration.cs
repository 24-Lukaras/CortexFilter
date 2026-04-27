using CortexFilter.Engine;
using CortexFilter.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CortexFilter.DependencyInjection;

internal class FromAssemblyDependencyRegistration
{
    private readonly IServiceCollection _services;
    private readonly IEnumerable<Type> _classes;
    public FromAssemblyDependencyRegistration(IServiceCollection services, Assembly assembly)
    {
        _services = services;
        _classes = assembly
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract)
            .ToList();
    }
    public void Register()
    {
        RegisterClientProviderFromAssembly();
        RegisterEnginesFromAssembly();
        RegisterFiltersFromAssembly();
        RegisterResourcesFromAssembly();
    }

    private void RegisterClientProviderFromAssembly()
    {
        var clientProviderType = typeof(IChatClientProvider);
        foreach (var type in _classes)
        {
            if (type.GetInterfaces().Any(x => x == clientProviderType))
            {
                _services.AddScoped(clientProviderType, type);
                break;
            }
        }
    }
    private void RegisterEnginesFromAssembly()
    {
        var baseType = typeof(NaturalLanguageEngine<>);
        var engineType = typeof(INaturalLanguageEngine<>);

        foreach (var type in _classes)
        {
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (!iface.IsGenericType)
                    continue;
                var genericTypeDef = iface.GetGenericTypeDefinition();
                if (genericTypeDef == engineType)
                {
                    _services.AddScoped(iface, type);
                }
            }
            if (type.BaseType is not null
                && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == baseType)
            {
                var typeArgs = type.BaseType.GetGenericArguments();
                var propInterface = typeof(INaturalLanguageEngineProperties<>).MakeGenericType(typeArgs[0]);
                var propImplementation = typeof(NaturalLanguageEngineProperties<>).MakeGenericType(typeArgs[0]);
                _services.AddScoped(propInterface, propImplementation);
            }
        }
    }
    private void RegisterFiltersFromAssembly()
    {
        var filterFactoryType = typeof(IConcreteFilterFactory<>);
        var ambiguousFilterType = typeof(AmbiguousFilter<>);
        foreach (var type in _classes)
        {
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (!iface.IsGenericType)
                    continue;
                var genericTypeDef = iface.GetGenericTypeDefinition();
                if (genericTypeDef == filterFactoryType)
                {
                    _services.AddScoped(iface, type);
                }
            }
            if (IsBaseTypeAmbiguousFilter(type, ambiguousFilterType, out var baseType))
            {
                _services.AddScoped(baseType, type);
            }
        }
    }
    private bool IsBaseTypeAmbiguousFilter(Type type, Type ambiguousFilterType, [NotNullWhen(true)] out Type? ambiguousType)
    {
        ambiguousType = null;
        if (type.BaseType is null)
            return false;
        var baseType = type.BaseType;
        if (type.BaseType.IsGenericType
            && type.BaseType.GetGenericTypeDefinition() == ambiguousFilterType)
        {
            ambiguousType = type.BaseType;
            return true;
        }
        return IsBaseTypeAmbiguousFilter(type.BaseType, ambiguousFilterType, out ambiguousType);
    }
    private void RegisterResourcesFromAssembly()
    {
        var ifaceResourceType = typeof(ICortexResource<>);
        foreach (var type in _classes)
        {
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (!iface.IsGenericType)
                    continue;
                var genericTypeDef = iface.GetGenericTypeDefinition();
                if (genericTypeDef == ifaceResourceType)
                {
                    _services.AddScoped(iface, type);
                }
            }
        }
    }
}
