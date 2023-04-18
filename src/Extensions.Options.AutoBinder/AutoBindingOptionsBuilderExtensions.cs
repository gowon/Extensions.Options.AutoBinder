namespace Extensions.Options.AutoBinder;

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

/// <summary>
///     Extension methods for automatically binding strongly typed options to data in configuration providers.
/// </summary>
public static class AutoBindingOptionsBuilderExtensions
{
    /// <summary>
    ///     Automatically binds an instance of <typeparamref name="TOptions" /> to data in configuration providers and adds it
    ///     to the DI container if the type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being configured.</typeparam>
    /// <param name="builder">The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" /> instance.</param>
    /// <param name="keys">
    ///     The list of keys to match when binding <typeparamref name="TOptions" /> to the configuration
    ///     instance.
    /// </param>
    /// <returns>The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
    public static OptionsBuilder<TOptions> AutoBind<TOptions>(this OptionsBuilder<TOptions> builder,
        params string[] keys)
        where TOptions : class
    {
        builder = builder ?? throw new ArgumentNullException(nameof(builder));
        var match = new List<string>();
        var attribute = typeof(TOptions).GetCustomAttribute<AutoBindAttribute>();

        if (keys.Length > 0)
        {
            match.AddRange(keys);
        }
        else if (attribute != null && attribute.Keys.Length > 0)
        {
            match.AddRange(attribute.Keys);
        }
        else
        {
            match.AddRange(ConfigurationBindingExtensions.GenerateKeyNames<TOptions>());
        }

        builder.Configure<IConfiguration>((option, configuration) => configuration.TryBind(option, match, out _));

        builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsChangeTokenSource<TOptions>), provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new ConfigurationChangeTokenSource<TOptions>(configuration);
        }));

        builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(TOptions), provider =>
        {
            var options = provider.GetRequiredService<IOptions<TOptions>>();
            return options.Value;
        }));

        return builder;
    }
}