namespace Extensions.Options.AutoBinder.Tests
{
    using System.Collections.Generic;
    using Fixtures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Xunit.Categories;

    [UnitTest]
    [Category("Configuration")]
    public class AutoBindingConfigurationExtensionsTests
    {
        [Fact]
        public void KeySetShouldBind()
        {
            // Arrange
            var keys = new[] { "Sample", "SampleOptions" };
            var provider =
                ServiceProviderFactory.CreateServiceProvider(nameof(SampleOptions), default, default, default,
                    default);
            var configuration = provider.GetRequiredService<IConfiguration>();

            // Act
            var result = configuration.TryBind(new SampleOptions(), keys, out var section);

            // Assert
            Assert.True(result);
            Assert.NotNull(section);
        }

        [Fact]
        public void NullKeysShouldFailToBind()
        {
            // Arrange
            var provider =
                ServiceProviderFactory.CreateServiceProvider(nameof(SampleOptions), default, default, default,
                    default);
            var configuration = provider.GetRequiredService<IConfiguration>();

            // Act
            var result = configuration.TryBind(new SampleOptions(), (IEnumerable<string>)null, out var section);

            // Assert
            Assert.False(result);
            Assert.Null(section);
        }
    }
}