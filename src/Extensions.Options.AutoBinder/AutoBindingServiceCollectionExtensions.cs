namespace Extensions.Options.AutoBinder;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extension methods for automatically binding strongly typed options to data in configuration providers.
/// </summary>
public static class AutoBindingServiceCollectionExtensions
{
    /// <summary>
    ///     Registers and binds strongly typed objects from the specified assemblies to data in configuration providers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AutoBindOptions(this IServiceCollection services)
    {
        return AutoBindOptions(services, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Registers and binds strongly typed objects from the specified assemblies to data in configuration providers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <param name="markerType">Marker type of the assembly to scan.</param>
    /// <param name="additionalTypes">Additional marker types of the assemblies to scan.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AutoBindOptions(this IServiceCollection services, Type markerType, params
        Type[] additionalTypes)
    {
        return AutoBindOptions(services, markerType.Assembly,
            additionalTypes.Select(type => type.Assembly).ToArray());
    }

    /// <summary>
    ///     Registers and binds strongly typed objects from the specified assemblies to data in configuration providers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <param name="additionalAssemblies">Additional assemblies to scan.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AutoBindOptions(this IServiceCollection services, Assembly assembly, params
        Assembly[] additionalAssemblies)
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));
        services.AddOptions();

        var assemblies = additionalAssemblies.Prepend(assembly).Distinct();
        var bindings = assemblies.SelectMany(a => a.GetTypes())
            .ToDictionary(type => type,
                type => type.GetCustomAttribute(typeof(AutoBindAttribute), false) as AutoBindAttribute)
            .Where(pair => pair.Value != null).Select(x => (x.Key, x.Value));

        var optionsMethod = typeof(OptionsServiceCollectionExtensions).GetMethods().Single(
            methodInfo =>
                methodInfo.Name == nameof(OptionsServiceCollectionExtensions.AddOptions) &&
                methodInfo.GetGenericArguments().Length == 1 &&
                methodInfo.GetParameters().Length == 1 &&
                methodInfo.GetParameters()[0].ParameterType == typeof(IServiceCollection));

        var binderMethod = typeof(AutoBindingOptionsBuilderExtensions).GetMethods().Single(
            methodInfo =>
                methodInfo.Name == nameof(AutoBindingOptionsBuilderExtensions.AutoBind) &&
                methodInfo.GetGenericArguments().Length == 1 &&
                methodInfo.GetParameters().Length == 3 &&
                methodInfo.GetParameters()[1].ParameterType == typeof(ServiceLifetime));

        foreach (var (optionsType, attribute) in bindings)
        {
            var genericOptionsMethod = optionsMethod.MakeGenericMethod(optionsType);
            var builder = genericOptionsMethod.Invoke(null, new object[] { services });
            var genericBinderMethod = binderMethod!.MakeGenericMethod(optionsType);
            genericBinderMethod.Invoke(null, new[] { builder, attribute!.ServiceLifetime, attribute!.Keys });
        }

        return services;
    }
}