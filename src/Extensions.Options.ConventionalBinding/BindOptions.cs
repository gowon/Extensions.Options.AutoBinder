namespace Extensions.Options.ConventionalBinding
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    ///     Implementation of <see cref="T:Microsoft.Extensions.Options.IConfigureOptions`1" /> that passes
    ///     <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> through dependency injection.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    public sealed class BindOptions<TOptions> : ConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="T:Extensions.Options.ConventionalBinding.BindOptions`1" /> with the
        ///     specified <paramref name="configuration" />.
        /// </summary>
        /// <param name="configuration">The configuration being bound.</param>
        public BindOptions(IConfiguration configuration) : base(options => { configuration.TryBind(options, out _); })
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}