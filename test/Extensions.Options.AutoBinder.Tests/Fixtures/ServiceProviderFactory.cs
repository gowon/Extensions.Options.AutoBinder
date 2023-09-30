namespace Extensions.Options.AutoBinder.Tests.Fixtures;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderFactory
{
    public static ServiceProvider CreateServiceProvider(string section, string stringVal, int intVal, bool boolVal,
        string dateVal, Action<IServiceCollection> setupContainer = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"{section}:StringVal", stringVal },
                { $"{section}:IntVal", intVal.ToString() },
                { $"{section}:BoolVal", boolVal.ToString() },
                { $"{section}:DateVal", dateVal }
            }).Build();

        var services = new ServiceCollection().AddSingleton<IConfiguration>(_ => configuration);
        setupContainer?.Invoke(services);
        return services.BuildServiceProvider();
    }
}