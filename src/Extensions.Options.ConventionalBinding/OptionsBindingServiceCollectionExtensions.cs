namespace Extensions.Options.ConventionalBinding
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    /// <summary>
    ///     Extension methods for adding configuration related strongly typed options to the DI container.
    /// </summary>
    public static class OptionsBindingServiceCollectionExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="markerTypes"></param>
        public static void BindOptionsFromAssembly(this IServiceCollection collection, params Type[] markerTypes)
        {
            collection.BindOptionsFromAssembly(markerTypes.Select(type => type.Assembly).ToArray());
        }

        /// <summary>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="assemblies"></param>
        public static void BindOptionsFromAssembly(this IServiceCollection collection, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface && t.Name.EndsWith("Options"))).ToList();

            foreach (var type in types)
            {
                collection.BindOptions(type);
            }
        }

        /// <summary>
        ///     Adds an instance of <typeparamref name="TOptions" /> to the <paramref name="collection" /> if the
        ///     type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> instance.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection BindOptions<TOptions>(this IServiceCollection collection)
            where TOptions : class
        {
            return collection.BindOptions(typeof(TOptions));
        }

        /// <summary>
        ///     Adds the specified <paramref name="type" /> to the <paramref name="collection" /> if the
        ///     type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> instance.</param>
        /// <param name="type">The <see cref="T:System.Type" /> of the strongly typed object.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</returns>
        public static IServiceCollection BindOptions(this IServiceCollection collection, Type type)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            AddChangeTokenSource(collection, type);
            AddBindOptions(collection, type);
            AddOptions(collection, type);
            return collection;
        }

        internal static void AddOptions(IServiceCollection collection, Type optionsType)
        {
            collection.TryAdd(new ServiceDescriptor(optionsType, provider =>
            {
                var genericType = typeof(IOptions<>).MakeGenericType(optionsType);
                dynamic options = provider.GetService(genericType);
                return options.Value;
            }, ServiceLifetime.Singleton));
        }

        internal static void AddBindOptions(IServiceCollection collection, Type optionsType)
        {
            var interfaceType = typeof(IConfigureOptions<>).MakeGenericType(optionsType);

            collection.TryAdd(new ServiceDescriptor(interfaceType, provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var type = typeof(BindOptions<>).MakeGenericType(optionsType);
                return Activator.CreateInstance(type, configuration);
            }, ServiceLifetime.Singleton));
        }

        internal static void AddChangeTokenSource(IServiceCollection collection, Type optionsType)
        {
            var interfaceType = typeof(IOptionsChangeTokenSource<>).MakeGenericType(optionsType);

            collection.TryAdd(new ServiceDescriptor(interfaceType, provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var type = typeof(ConfigurationChangeTokenSource<>).MakeGenericType(optionsType);
                return Activator.CreateInstance(type, configuration);
            }, ServiceLifetime.Singleton));
        }
    }
}