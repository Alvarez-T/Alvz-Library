using AtoZ.Extensions.Configuration.Exceptions;
using AtoZ.Extensions.Configuration.Ini;
using AtoZ.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AtoZ.Extensions.Configuration;

public static class WritableConfigurationExtensions
{
    public static IConfigurationBuilder AddWritableJsonFile<T>(this IConfigurationBuilder builder, string path)
        where T : class
    {
        var source = new JsonWritableConfigurationSource<T>()
        {
            Path = path,
            Optional = false,
            ReloadOnChange = false,
        };

        builder.Add(source);

        return builder;
    }

    public static IConfigurationBuilder AddWritableIniFile<T>(this IConfigurationBuilder builder, string path)
        where T : class
    {
        var source = new IniWritableConfigurationSource<T>()
        {
            Path = path,
            Optional = false,
            ReloadOnChange = false,
        };

        builder.Add(source);

        return builder;
    }

    public static void ConfigureWritable<T>(this IServiceCollection services,
                                         string section,
                                         IConfiguration configuration)
                                         where T : class, new()
    {
        services.Configure<T>(configuration.GetRequiredSection(section));

        var writableConfigurationProvider = ((ConfigurationRoot)configuration).Providers
            .OfType<IWritableConfigurationProvider<T>>().First() ?? throw new ConfigurationProviderNotFounded(typeof(T));

        services.AddTransient<IWritableOptions<T>>(provider =>
        {
            var options = provider.GetRequiredService<IOptionsMonitor<T>>();
            return new WritableOptions<T>(options, writableConfigurationProvider);
        });
    }
}
