namespace Extensions.Options.ConventionalBinding.Tests
{
    using System;
    using System.Collections.Generic;
    using Fixtures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Models;
    using Xunit;
    using Xunit.Categories;

    [UnitTest]
    public class ServiceCollectionExtensionsTests
    {
        private const string BindOptionsCollectionParameterName = "collection";
        private const string BindOptionsTypeParameterName = "type";

        [Fact]
        public void TryBind_ReturnFalse()
        {
            var configuration = new ConfigurationBuilder().Build();
            var result = configuration.TryBind(new SampleOptions(), out _);
            Assert.False(result);
        }

        [Fact]
        public void BindOptions_ThrowsIfCollectionIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                OptionsBindingServiceCollectionExtensions.BindOptions(null, typeof(SampleOptions)));
            Assert.StartsWith(BindOptionsCollectionParameterName, ex.ParamName!);
        }

        [Fact]
        public void BindOptions_ThrowsIfTypeIsNull()
        {
            var services = new ServiceCollection();
            var exception = Assert.Throws<ArgumentNullException>(() =>
                services.BindOptions(null));

            Assert.NotNull(exception);
            Assert.StartsWith(BindOptionsTypeParameterName, exception.ParamName!);
        }

        [Theory]
        [SampleOptionsData]
        public void BindFieldsToChangeTrackedObject(string stringVal, int intVal, bool boolVal, string dateVal)
        {
            // Arrange
            var provider = CreateServiceProvider(stringVal, intVal, boolVal, dateVal);

            // Act
            var monitor = provider.GetService<IOptionsMonitor<SampleOptions>>();
            var options = monitor!.CurrentValue;

            // Assert
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        [Theory]
        [SampleOptionsData(10)]
        public void BindFieldsToObject(string stringVal, int intVal, bool boolVal, string dateVal)
        {
            // Arrange
            var provider = CreateServiceProvider(stringVal, intVal, boolVal, dateVal);

            // Act
            var options = provider.GetService<SampleOptions>();

            // Assert
            Assert.NotNull(options);
            Assert.Equal(stringVal, options.StringVal);
            Assert.Equal(intVal, options.IntVal);
            Assert.Equal(boolVal, options.BoolVal);
            Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
        }

        private static ServiceProvider CreateServiceProvider(string stringVal, int intVal, bool boolVal, string dateVal)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Sample:StringVal", stringVal },
                    { "Sample:IntVal", intVal.ToString() },
                    { "Sample:BoolVal", boolVal.ToString() },
                    { "Sample:DateVal", dateVal }
                }).Build();

            var services = new ServiceCollection();
            services.AddScoped<IConfiguration>(_ => configuration);
            services.AddOptions();
            services.BindOptions<SampleOptions>();
            services.BindOptionsFromAssembly(typeof(SampleOptions));
            services.BindOptionsFromAssembly(typeof(SampleOptions).Assembly);
            services.BindOptionsFromAssembly("Options", typeof(SampleOptions));
            return services.BuildServiceProvider();
        }
    }
}