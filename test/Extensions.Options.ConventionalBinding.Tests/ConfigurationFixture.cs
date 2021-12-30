namespace Extensions.Options.ConventionalBinding.Tests
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    public class ConfigurationFixture
    {
        public IConfiguration Configuration { get; }

        public ConfigurationFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Sample:StringVal", "Orange"},
                    {"Sample:IntVal", "999"},
                    {"Sample:BoolVal", "true"},
                    {"Sample:DateVal", "2020-07-11T07:43:29-4:00"}
                }).Build();
        }
    }
}