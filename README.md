# Extensions.Options.ConventionalBinding

[![codecov](https://codecov.io/gh/gowon/Extensions.Options.ConventionalBinding/branch/main/graph/badge.svg?token=S3RDWZCKWF)](https://codecov.io/gh/gowon/Extensions.Options.ConventionalBinding)

Provides additional functionality to related to binding Options by convention using dependency injection.

## Install

|Package|Stable|Preview|
|-|-|-|
|Extensions.Options.ConventionalBinding||[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fgowon%2Fpre-release%2Fshield%2FExtensions.Options.ConventionalBinding%2Flatest)](https://f.feedz.io/gowon/pre-release/packages/Extensions.Options.ConventionalBinding/latest/download)|

The package can be obtained by either locally cloning this git repository and building it or via NuGet/Feedz:

```shell
Install-Package Extensions.Options.ConventionalBinding
```

or

```shell
dotnet add package Extensions.Options.ConventionalBinding
```

## Usage

Add Options binding to your service collection:

```csharp
services.AddOptions();
services.BindOptionsFromAssemblies(Assembly.GetExecutingAssembly());
```

Given an Options class like:

```csharp
public class SampleOptions
{
    public string StringVal { get; set; }
    public int IntVal { get; set; }
    public bool BoolVal { get; set; }
    public DateTime? DateVal { get; set; }
}
```

The following configuration section will be automatically binded:

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

## License

MIT