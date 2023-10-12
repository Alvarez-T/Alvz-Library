using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace AtoZ.Extensions.Configuration.Json;

public class JsonWritableConfigurationSource<T> : JsonConfigurationSource
    where T : class
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new JsonWritableConfigurationProvider<T>(this);
    }
}
