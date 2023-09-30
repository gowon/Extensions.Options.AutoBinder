namespace Extensions.Options.AutoBinder.Tests;

using Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Categories;

[UnitTest("Binding")]
public class ConfigurationBindingExtensionsTests
{
    [Theory]
    [OptionsData(5, "Sample")]
    [OptionsData(5, "SampleOptions")]
    public void ShouldBindToGeneratedKeys(string stringVal, int intVal, bool boolVal, string dateVal,
        string configurationSectionName)
    {
        // Arrange
        var options = new SampleOptions();
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal,
                dateVal);
        var configuration = provider.GetRequiredService<IConfiguration>();

        // Act
        var result = configuration.TryBind(options, out var section);

        // Assert
        Assert.True(result);
        Assert.NotNull(section);
        Assert.Equal(configurationSectionName, section.Key);
        Assert.Equal(stringVal, options.StringVal);
        Assert.Equal(intVal, options.IntVal);
        Assert.Equal(boolVal, options.BoolVal);
        Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
    }

    [Theory]
    [OptionsData(5, "Sample")]
    [OptionsData(5, "SampleOptions")]
    public void ShouldBindToGivenKeys(string stringVal, int intVal, bool boolVal, string dateVal,
        string configurationSectionName)
    {
        // Arrange
        var options = new SampleOptions();
        var keys = new[] { "Sample", "SampleOptions" };
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal, intVal, boolVal,
                dateVal);
        var configuration = provider.GetRequiredService<IConfiguration>();

        // Act
        var result = configuration.TryBind(options, keys, out var section);

        // Assert
        Assert.True(result);
        Assert.NotNull(section);
        Assert.Equal(configurationSectionName, section.Key);
        Assert.Equal(stringVal, options.StringVal);
        Assert.Equal(intVal, options.IntVal);
        Assert.Equal(boolVal, options.BoolVal);
        Assert.Equal(DateTime.Parse(dateVal), options.DateVal);
    }

    [Fact]
    public void ShouldNotBindToNullSet()
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