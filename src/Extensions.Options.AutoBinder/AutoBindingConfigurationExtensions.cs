﻿namespace Extensions.Options.AutoBinder
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     Extension methods that allows binding strongly typed objects to configuration values.
    /// </summary>
    public static class AutoBindingConfigurationExtensions
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
            out IConfiguration foundSection)
            where TOptions : class
        {
            return TryBind(configuration, options, null, out foundSection);
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
            string[] keys, out IConfiguration foundSection)
            where TOptions : class
        {
            foundSection = null;
            var sectionKeys = keys?.ToList() ?? new List<string>();

            if (sectionKeys.Count == 0)
            {
                var name = typeof(TOptions).Name;
                sectionKeys.Add(name);

                if (name.EndsWith(Constants.DefaultOptionsSuffix))
                {
                    sectionKeys.Add(name.Remove(name.Length - Constants.DefaultOptionsSuffix.Length));
                }
                else
                {
                    sectionKeys.Add($"{name}{Constants.DefaultOptionsSuffix}");
                }
            }

            foreach (var key in sectionKeys)
            {
                var section = configuration.GetSection(key);
                if (section.Exists())
                {
                    foundSection = section;
                    configuration.Bind(key, options);
                    return true;
                }
            }

            return false;
        }
    }
}