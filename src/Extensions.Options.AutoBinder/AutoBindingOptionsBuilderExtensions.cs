namespace Extensions.Options.AutoBinder
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    ///     Extension methods for automatically binding strongly typed options to data in configuration providers.
    /// </summary>
    public static class AutoBindingOptionsBuilderExtensions
    {
        /// <summary>
        ///     Automatically binds an instance of <typeparamref name="TOptions" /> to data in configuration providers and adds it
        ///     to the DI container if the
        ///     type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" /> instance.</param>
        /// <param name="suffix">
        ///     The suffix to be removed from the type name when binding <typeparamref name="TOptions" /> to the
        ///     configuration instance.
        /// </param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Options.OptionsBuilder`1" />.</returns>
        public static OptionsBuilder<TOptions> AutoBind<TOptions>(this OptionsBuilder<TOptions> builder,
            string suffix = Constants.DefaultOptionsSuffix)
            where TOptions : class
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Configure<IConfiguration>((option, configuration) => configuration.TryBind(option, suffix, out _));

            builder.Services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                return new ConfigurationChangeTokenSource<TOptions>(configuration);
            });

            builder.Services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<TOptions>>();
                return options.Value;
            });

            return builder;
        }
    }
}