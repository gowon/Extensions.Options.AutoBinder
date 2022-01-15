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
    [Category("Declarative")]
    public class DeclarativeAutoBindingTests
    {
        [Theory]
        [OptionsData(5, "MySettings")]
        [OptionsData(5, "OrangesAndBananas")]
        public void CanBindToIOptionsMonitor(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var monitor = provider.GetService<IOptionsMonitor<OtherSampleOptions>>();

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
        [OptionsData(5, "MySettings")]
        [OptionsData(5, "OrangesAndBananas")]
        public void CanBindToIOptionsSnapshot(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var service = provider.GetService<IOptionsSnapshot<OtherSampleOptions>>();

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
        [OptionsData(5, "MySettings")]
        [OptionsData(5, "OrangesAndBananas")]
        public void CanBindToIOptions(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var service = provider.GetService<IOptions<OtherSampleOptions>>();

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
        [OptionsData(10, "MySettings")]
        [OptionsData(10, "OrangesAndBananas")]
        public void CanBindToSingletonObject(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var options = provider.GetService<OtherSampleOptions>();

            // Assert
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Theory]
        [OptionsData(1, "OtherSample")]
        [OptionsData(1, "OtherSampleOptions")]
        [OptionsData(1, "ZXXZXYXYXZ")]
        public void ShouldFailToBind(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider = CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal, dateVal);

            // Act
            var options = provider.GetService<OtherSampleOptions>();

            // Assert
            Assert.NotNull(options);
            Assert.NotEqual(stringVal, options.StringVal);
            Assert.NotEqual(intVal, options.IntVal);
            Assert.NotEqual(boolVal, options.BoolVal);
            Assert.NotEqual(DateTime.Parse(dateVal), options.DateVal);
        }

        private static ServiceProvider CreateServiceProvider(string section, string stringVal, int intVal, bool boolVal,
            string dateVal)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"{section}:StringVal", stringVal },
                    { $"{section}:IntVal", intVal.ToString() },
                    { $"{section}:BoolVal", boolVal.ToString() },
                    { $"{section}:DateVal", dateVal }
                }).Build();

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(_ => configuration)
                .AutoBindOptions()
                .AutoBindOptions(typeof(OtherSampleOptions), typeof(OtherStuff));

            return services.BuildServiceProvider();
        }
    }
}