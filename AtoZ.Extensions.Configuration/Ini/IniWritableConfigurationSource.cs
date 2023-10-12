using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;

namespace AtoZ.Extensions.Configuration.Ini;

public class IniWritableConfigurationSource<T> : IniConfigurationSource
    where T : class
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new IniWritableConfigurationProvider<T>(this);
    }
}
