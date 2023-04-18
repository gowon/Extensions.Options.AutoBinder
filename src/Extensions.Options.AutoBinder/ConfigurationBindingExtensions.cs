namespace Extensions.Options.AutoBinder;

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

/// <summary>
///     Extension methods that allows binding strongly typed objects to configuration values.
/// </summary>
public static class ConfigurationBindingExtensions
{
    /// <summary>
    ///     Attempts to bind the given object instance to the configuration section specified by the key by matching property
    ///     names against configuration keys recursively.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being configured.</typeparam>
    /// <param name="configuration">The configuration instance to bind.</param>
    /// <param name="options">The instance of <typeparamref name="TOptions" /> to bind.</param>
    /// <param name="foundSection">
    ///     When this method returns, contains the matching
    ///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> object, or null if a matching section does not
    ///     exist. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <returns>
    ///     true if <paramref name="options">s</paramref> was bound to the configuration instance successfully; otherwise,
    ///     false.
    /// </returns>
    public static bool TryBind<TOptions>(this IConfiguration configuration, TOptions options,
        out IConfigurationSection foundSection)
        where TOptions : class
    {
        return configuration.TryBind(options, GenerateKeyNames<TOptions>(), out foundSection);
    }

    /// <summary>
    ///     Attempts to bind the given object instance to the configuration section specified by the key by matching property
    ///     names against configuration keys recursively.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being configured.</typeparam>
    /// <param name="configuration">The configuration instance to bind.</param>
    /// <param name="options">The instance of <typeparamref name="TOptions" /> to bind.</param>
    /// <param name="key">The key to match when <typeparamref name="TOptions" /> to the configuration instance.</param>
    /// <param name="foundSection">
    ///     When this method returns, contains the matching
    ///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> object, or null if a matching section does not
    ///     exist. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <returns>
    ///     true if <paramref name="options">s</paramref> was bound to the configuration instance successfully; otherwise,
    ///     false.
    /// </returns>
    public static bool TryBind<TOptions>(this IConfiguration configuration, TOptions options, string key,
        out IConfigurationSection foundSection)
        where TOptions : class
    {
        foundSection = null;
        var section = configuration.GetSection(key);
        if (section.Exists())
        {
            foundSection = section;
            configuration.Bind(key, options);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Attempts to bind the given object instance to the configuration section specified by the key by matching property
    ///     names against configuration keys recursively.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being configured.</typeparam>
    /// <param name="configuration">The configuration instance to bind.</param>
    /// <param name="options">The instance of <typeparamref name="TOptions" /> to bind.</param>
    /// <param name="keys">The list of keys to match when <typeparamref name="TOptions" /> to the configuration instance.</param>
    /// <param name="foundSection">
    ///     When this method returns, contains the matching
    ///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> object, or null if a matching section does not
    ///     exist. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <returns>
    ///     true if <paramref name="options">s</paramref> was bound to the configuration instance successfully; otherwise,
    ///     false.
    /// </returns>
    public static bool TryBind<TOptions>(this IConfiguration configuration, TOptions options,
        IEnumerable<string> keys, out IConfigurationSection foundSection)
        where TOptions : class
    {
        foundSection = null;
        if (keys == null)
        {
            return false;
        }

        foreach (var key in keys)
        {
            var found = configuration.TryBind(options, key, out foundSection);
            if (found)
            {
                return true;
            }
        }

        return false;
    }

    internal static IEnumerable<string> GenerateKeyNames<TOptions>()
    {
        var keys = new List<string>();
        var name = typeof(TOptions).Name;
        keys.Add(name);

        keys.Add(name.EndsWith(Constants.DefaultOptionsSuffix)
            ? name.Remove(name.Length - Constants.DefaultOptionsSuffix.Length)
            : $"{name}{Constants.DefaultOptionsSuffix}");

        return keys;
    }
}