# Extensions.Options.AutoBinder

[![Nuget (with prereleases)](https://img.shields.io/nuget/v/Extensions.Options.AutoBinder)](https://www.nuget.org/packages/Extensions.Options.AutoBinder)
[![Nuget download count badge](https://img.shields.io/nuget/dt/Extensions.Options.AutoBinder)](https://www.nuget.org/packages/Extensions.Options.AutoBinder)
[![codecov](https://codecov.io/gh/gowon/Extensions.Options.AutoBinder/branch/main/graph/badge.svg?token=KHH2IJHLBD)](https://codecov.io/gh/gowon/Extensions.Options.AutoBinder)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fgowon%2Fpre-release%2Fshield%2FExtensions.Options.AutoBinder%2Flatest)](https://f.feedz.io/gowon/pre-release/packages/Extensions.Options.AutoBinder/latest/download)

Provides additional functionality related to automatically binding strongly typed options to data in configuration providers.

## Install

Add a reference to the [`Extensions.Options.AutoBinder`](https://www.nuget.org/packages/Extensions.Options.AutoBinder) package:

```shell
dotnet add package Extensions.Options.AutoBinder
```

## Usage

Registering your options gives you access to the following from the dependency injection container:

- `TOptions` - Same as `IOptions<TOptions>`, it represents configuration data once when the application starts and any changes in configuration will require the application to be restarted. It is unwrapped from the `IOptions<>` interface so that consuming interfaces do not have to force a dependency on the pattern. It is registered in the dependency injection container with a singleton lifetime.

- `IOptions<TOptions>` - Represents configuration data once when the application starts and any changes in configuration will require the application to be restarted. It is registered in the dependency injection container with a singleton lifetime.

- `IOptionsSnapshot<TOptions>` - Represents configuration on every request. Any changes in configuration while the application is running will be available for new requests without the need to restart the application. It is registered in the dependency injection container as a scoped lifetime.

- `IOptionsMonitor<TOptions>` - Is a service used to retrieve options and manage options notifications for TOptions instances. It is registered in the dependency injection container as a singleton lifetime.

### Conventional Binding

Create a strongly typed objects that you want to bind to the configuration provider:

```csharp
public class SampleOptions
{
    public string StringVal { get; set; }
    public int IntVal { get; set; }
    public bool BoolVal { get; set; }
    public DateTime? DateVal { get; set; }
}

public class MoreSampleOptions
{
    public string StringVal { get; set; }
    public int IntVal { get; set; }
    public bool BoolVal { get; set; }
    public DateTime? DateVal { get; set; }
}
```

Strongly typed options are registered as described in the [Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options). Simply add `.AutoBind()` to the Options builder when registering your options:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // By default, will bind to "Sample" or "SampleOptions"
    services.AddOptions<SampleOptions>().AutoBind();
    
    // or, you can specify which keys to bind to
    services.AddOptions<MoreSampleOptions>().AutoBind("SectionB", "NewConfig");
}
```

The library will attempt to match the strongly typed object to a configuration section following a simple convention: Using the type name of the object with and without the suffix `Options`. In the case of our example class, it will be bound to any section in the configuration with the name `Sample` or `SampleOptions`. The following JSON segment would be successfully bound to the `SampleOptions` class:

```json
{
  "Sample": {
    "StringVal": "Orange",
    "IntVal": 999,
    "BoolVal": true,
    "DateVal": "2020-07-11T07:43:29-4:00"
  },
  "SectionB": {
    "StringVal": "Orange",
    "IntVal": 999,
    "BoolVal": true,
    "DateVal": "2020-07-11T07:43:29-4:00"
  }
}
```

### Declarative Binding

Create strongly typed objects and apply the `AutoBind` attribute to the ones that you want to bind to the configuration provider. There is an optional parameter to specify the keys that you would like to bind to in the configuration:

```csharp
[AutoBind("Squirrels", "Settings")]
public class OtherSampleOptions
{
    public string StringVal { get; set; }
    public int? IntVal { get; set; }
    public bool? BoolVal { get; set; }
    public DateTime? DateVal { get; set; }
}

[AutoBind]
public class OtherStuff
{
    public string StringVal { get; set; }
    public int? IntVal { get; set; }
    public bool? BoolVal { get; set; }
    public DateTime? DateVal { get; set; }
}
```

You can scan one or more assemblies for types that have been decorated with the `AutoBind` attribute by using the registration helper `AutoBindOptions()`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // By default, will scan the calling assembly for options to bind
    services.AutoBindOptions();
    
    // or, you can specify one or more assemblies to scan
    services.AutoBindOptions(typeof(SampleOptions), typeof(OtherSampleOptions));
}
```

The library will attempt to match all strongly typed objects  a configuration section using the default convention unless keys are specified; each key will be attempted in the order they are declared in the attribute. In the following JSON example, the `OtherSampleOptions` object would be bound to the `Settings` section and `OtherStuff` object would be bound to the `OtherStuffOptions` section:

```json
{
  "Settings": {
    "StringVal": "Orange",
    "IntVal": 999,
    "BoolVal": true,
    "DateVal": "2020-07-11T07:43:29-4:00"
  },
  "OtherStuffOptions": {
    "StringVal": "Orange",
    "IntVal": 999,
    "BoolVal": true,
    "DateVal": "2020-07-11T07:43:29-4:00"
  }
}
```
