namespace Extensions.Options.AutoBinder.Tests
{
    using System;
    using Fixtures;
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
        public void CanBindToIOptionsMonitor(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider =
                ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                    intVal, boolVal, dateVal, collection => { collection.AddOptions<SampleOptions>().AutoBind(); });

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
        public void CanBindToIOptionsSnapshot(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider =
                ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                    intVal, boolVal, dateVal, collection => { collection.AddOptions<SampleOptions>().AutoBind(); });

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
        public void CanBindToIOptions(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider =
                ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                    intVal, boolVal, dateVal, collection => { collection.AddOptions<SampleOptions>().AutoBind(); });

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
        public void CanBindToSingletonObject(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider =
                ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                    intVal, boolVal, dateVal, collection => { collection.AddOptions<SampleOptions>().AutoBind(); });

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
        public void CanBindToSingletonObjectAlternative(string stringVal, int intVal, bool boolVal, string dateVal,
            string configurationSectionName)
        {
            // Arrange
            var provider =
                ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                    intVal, boolVal, dateVal, collection => { collection.AddOptions<OtherStuff>().AutoBind(configurationSectionName); });

            // Act
            var options = provider.GetService<OtherStuff>();

            // Assert
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }
    }
}