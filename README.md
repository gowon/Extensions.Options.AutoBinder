# Extensions.Options.AutoBinder

[![Nuget](https://img.shields.io/nuget/dt/Extensions.Options.AutoBinder?color=blue)](https://www.nuget.org/packages/Extensions.Options.AutoBinder)
[![codecov](https://codecov.io/gh/gowon/Extensions.Options.AutoBinder/branch/main/graph/badge.svg?token=KHH2IJHLBD)](https://codecov.io/gh/gowon/Extensions.Options.AutoBinder)

Provides additional functionality related to automatically binding strongly typed options to data in configuration providers.

## Install

|Package|Stable|Preview|
|-|-|-|
|Extensions.Options.AutoBinder|[![Nuget (with prereleases)](https://img.shields.io/nuget/v/Extensions.Options.AutoBinder?color=blue)](https://www.nuget.org/packages/Extensions.Options.AutoBinder)|[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fgowon%2Fpre-release%2Fshield%2FExtensions.Options.AutoBinder%2Flatest)](https://f.feedz.io/gowon/pre-release/packages/Extensions.Options.AutoBinder/latest/download)|

The package can be obtained by either locally cloning this git repository and building it or via NuGet/Feedz:

```shell
Install-Package Extensions.Options.AutoBinder
```

or

```shell
dotnet add package Extensions.Options.AutoBinder
```

## Usage

Create a strongly typed object that you want to bind to the configuration provider:

```csharp
public class SampleOptions
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
    \\...

    services.AddOptions<SampleOptions>().AutoBind();
}
```

The optional parameter `suffix` can be used to indicated a suffix phrase (default: `Options`) that can be removed from the class name while binding. This allows the tooling to match the option class to a configuration section without the suffix phrase.

For example, the following JSON segment would be successfully bound to the `SampleOptions` class:

```json
{
  "Sample": {
    "StringVal": "Orange",
    "IntVal": 999,
    "BoolVal": true,
    "DateVal": "2020-07-11T07:43:29-4:00"
  }
}
```

This gives you access to the following from the dependency injection container:

- `IOptions<TOptions>` - Represents configuration data once when the application starts and any changes in configuration will require the application to be restarted. It is registered in the dependency injection container with a singleton lifetime.

- `TOptions` - Same as `IOptions<TOptions>`, it represents configuration data once when the application starts and any changes in configuration will require the application to be restarted. It is unwrapped from the `IOptions<>` interface so that consuming interfaces do not have to force a dependency on the pattern. It is registered in the dependency injection container with a singleton lifetime.

- `IOptionsSnapshot<TOptions>` - Represents configuration on every request. Any changes in configuration while the application is running will be available for new requests without the need to restart the application. It is registered in the dependency injection container as a scoped lifetime.

- `IOptionsMonitor<TOptions>` - Is a service used to retrieve options and manage options notifications for TOptions instances. It is registered in the dependency injection container as a singleton lifetime.

## License

MIT
