namespace Extensions.Options.AutoBinder.Tests
{
    using System;
    using System.Collections.Generic;
    using Fixtures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Xunit;
    using Xunit.Categories;

    [UnitTest]
    [Category("Conventional")]
    public class ConventionalAutoBindingTests
    {
        [Theory]
        [OptionsData(5, "Sample")]
        [OptionsData(5, "SampleOptions")]
        public void CanBindToIOptionsMonitor(string stringVal, int intVal, bool boolVal, string dateVal, string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider<SampleOptions>(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var monitor = provider.GetService<IOptionsMonitor<SampleOptions>>();

            // Assert
            Assert.NotNull(monitor);
            
            var options = monitor.CurrentValue;
            
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Theory]
        [OptionsData(5, "Sample")]
        [OptionsData(5, "SampleOptions")]
        public void CanBindToIOptionsSnapshot(string stringVal, int intVal, bool boolVal, string dateVal, string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider<SampleOptions>(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var service = provider.GetService<IOptionsSnapshot<SampleOptions>>();

            // Assert
            Assert.NotNull(service);

            var options = service.Value;

            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Theory]
        [OptionsData(5, "Sample")]
        [OptionsData(5, "SampleOptions")]
        public void CanBindToIOptions(string stringVal, int intVal, bool boolVal, string dateVal, string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider<SampleOptions>(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var service = provider.GetService<IOptions<SampleOptions>>();

            // Assert
            Assert.NotNull(service);

            var options = service.Value;

            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Theory]
        [OptionsData(5, "Sample")]
        [OptionsData(5, "SampleOptions")]
        public void CanBindToSingletonObject(string stringVal, int intVal, bool boolVal, string dateVal, string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider<SampleOptions>(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var options = provider.GetService<SampleOptions>();

            // Assert
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Theory]
        [OptionsData(5, "OtherStuff")]
        [OptionsData(5, "OtherStuffOptions")]
        public void CanBindToSingletonObjectAlternative(string stringVal, int intVal, bool boolVal, string dateVal, string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider<OtherStuff>(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var options = provider.GetService<OtherStuff>();

            // Assert
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Fact]
        public void ShouldFailToBindToEmptyConfiguration()
        {
            // Arrange
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var result = configuration.TryBind(new SampleOptions(), out var foundSection);

            // Assert
            Assert.False(result);
            Assert.Null(foundSection);
        }

        private static ServiceProvider CreateServiceProvider<TOptions>(string section, string stringVal, int intVal, bool boolVal, string dateVal)
        where TOptions : SampleOptions
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"{section}:StringVal", stringVal },
                    { $"{section}:IntVal", intVal.ToString() },
                    { $"{section}:BoolVal", boolVal.ToString() },
                    { $"{section}:DateVal", dateVal }
                }).Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddOptions<TOptions>().AutoBind();
            return services.BuildServiceProvider();
        }
    }
}