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
    public class AutoBindingOptionsBuilderExtensionsTests
    {
        [Fact]
        public void TryBind_ReturnFalse()
        {
            var configuration = new ConfigurationBuilder().Build();
            var result = configuration.TryBind(new SampleOptions(), out _);
            Assert.False(result);
        }

        [Theory]
        [SampleOptionsData(10)]
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
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddOptions<SampleOptions>().AutoBind();
            return services.BuildServiceProvider();
        }
    }
}