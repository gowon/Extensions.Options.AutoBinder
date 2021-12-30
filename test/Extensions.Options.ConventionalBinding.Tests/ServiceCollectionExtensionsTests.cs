namespace Extensions.Options.ConventionalBinding.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Xunit;

    public class ServiceCollectionExtensionsTests : IClassFixture<ConfigurationFixture>
    {
        private ConfigurationFixture _fixture;

        public ServiceCollectionExtensionsTests(ConfigurationFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(typeof(SampleOptions), true)]
        public void TryBindScenarios(Type optionsType, bool bindResult)
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                OptionsBindingServiceCollectionExtensions.BindOptions(null, typeof(SampleOptions)));
            Assert.StartsWith("services", ex.ParamName);
        }

        [Fact]
        public void BindOptions_ThrowsIfServicesIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                OptionsBindingServiceCollectionExtensions.BindOptions(null, typeof(SampleOptions)));
            Assert.StartsWith("services", ex.ParamName);
        }

        [Fact]
        public void BindOptions_ThrowsIfTypeIsNull()
        {
            var services = new ServiceCollection();
            var ex = Assert.Throws<ArgumentNullException>(() =>
                services.BindOptions(null));
            Assert.StartsWith("type", ex.ParamName);
        }

        [Fact]
        public void BindOptionsToConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Sample:StringVal", "Orange" },
                    { "Sample:IntVal", "999" },
                    { "Sample:BoolVal", "true" },
                    { "Sample:DateVal", "2020-07-11T07:43:29-4:00" }
                }).Build();

            var services = new ServiceCollection();
            services.AddScoped<IConfiguration>(_ => configuration);
            services.AddOptions();
            services.BindOptions<SampleOptions>();
            var provider = services.BuildServiceProvider();

            SampleOptions options;
            using (var scope = provider.CreateScope())
            {
                options = scope.ServiceProvider.GetRequiredService<SampleOptions>();
            }

            Assert.NotNull(options);
            Assert.Equal(999, options.IntVal);
        }
    }
}