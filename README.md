# Extensions.Options.ConventionalBinding

## Usage

Add package to project:

```shell
Install-Package Extensions.Options.ConventionalBinding
```

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