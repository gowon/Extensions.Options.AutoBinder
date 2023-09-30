namespace Extensions.Options.AutoBinder.Tests;

using Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Categories;

[UnitTest("Declarative")]
public class DeclarativeAutoBindingTests
{
    [Theory]
    [OptionsData(5, "MySettings")]
    [OptionsData(5, "OrangesAndBananas")]
    public void CanBindToIOptionsMonitor(string stringVal, int intVal, bool boolVal, string dateVal,
        string configurationSectionName)
    {
        // Arrange
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                intVal, boolVal, dateVal, collection => { collection.AutoBindOptions(); });

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
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                intVal, boolVal, dateVal, collection => { collection.AutoBindOptions(); });

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
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                intVal, boolVal, dateVal, collection => { collection.AutoBindOptions(); });

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
    [OptionsData(5, "MySettings")]
    [OptionsData(5, "OrangesAndBananas")]
    public void CanBindToSingletonObject(string stringVal, int intVal, bool boolVal, string dateVal,
        string configurationSectionName)
    {
        // Arrange
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                intVal, boolVal, dateVal, collection => { collection.AutoBindOptions(); });

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
    [OptionsData(5, "OtherStuff")]
    [OptionsData(5, "OtherStuffOptions")]
    public void CanBindToSingletonObjectAlternative(string stringVal, int intVal, bool boolVal, string dateVal,
        string configurationSectionName)
    {
        // Arrange
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                intVal, boolVal, dateVal,
                collection => { collection.AutoBindOptions(typeof(SampleOptions), typeof(OtherSampleOptions)); });

        // Act
        var options = provider.GetService<OtherStuff>();

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
        var provider =
            ServiceProviderFactory.CreateServiceProvider(configurationSectionName, stringVal,
                intVal, boolVal, dateVal, collection => { collection.AutoBindOptions(); });

        // Act
        var options = provider.GetService<OtherSampleOptions>();

        // Assert
        Assert.NotNull(options);
        Assert.NotEqual(stringVal, options.StringVal);
        Assert.NotEqual(intVal, options.IntVal);
        Assert.NotEqual(boolVal, options.BoolVal);
        Assert.NotEqual(DateTime.Parse(dateVal), options.DateVal);
    }
}