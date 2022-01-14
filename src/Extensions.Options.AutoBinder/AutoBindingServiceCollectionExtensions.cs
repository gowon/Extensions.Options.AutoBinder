namespace Extensions.Options.AutoBinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class AutoBindingServiceCollectionExtensions
    {
        public static IServiceCollection AutoBindOptions(this IServiceCollection services, params
            Assembly[] assemblies)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            var list = assemblies.Length == 0
                ? new List<Assembly>
                {
                    Assembly.GetCallingAssembly()
                }
                : assemblies.ToList();

            var types = list.SelectMany(GetTypesWithAttribute<AutoBindAttribute>);

            services.AddOptions();
            
            var optionsMethod = typeof(OptionsServiceCollectionExtensions).GetMethods().Single(
                m =>
                    m.Name == nameof(OptionsServiceCollectionExtensions.AddOptions) &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(IServiceCollection));

            var binderMethod = typeof(AutoBindingOptionsBuilderExtensions).GetMethod(
                nameof(AutoBindingOptionsBuilderExtensions.AutoBind),
                BindingFlags.Static | BindingFlags.Public);

            foreach (var optionsType in types)
            {
                //var optionsMethod = typeof(OptionsServiceCollectionExtensions).GetMethod(
                //    nameof(OptionsServiceCollectionExtensions.AddOptions),
                //    BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(IServiceCollection) }, null);
                var genericOptionsMethod = optionsMethod.MakeGenericMethod(optionsType);
                var builder = genericOptionsMethod.Invoke(null, new object[] { services });

                //var binderMethod = typeof(AutoBindingOptionsBuilderExtensions).GetMethod(
                //    nameof(AutoBindingOptionsBuilderExtensions.AutoBind),
                //    BindingFlags.Static | BindingFlags.Public);
                var genericBinderMethod = binderMethod.MakeGenericMethod(optionsType);
                genericBinderMethod.Invoke(null, new[] { builder, Constants.DefaultOptionsSuffix });
            }

            return services;
        }

        private static IEnumerable<Type> GetTypesWithAttribute<TAttribute>(Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(TAttribute), true).Length > 0);
        }
    }
}