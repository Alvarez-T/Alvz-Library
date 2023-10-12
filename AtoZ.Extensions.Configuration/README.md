
# A - Z Extensions Configuration

This library has some extensions targeting the [_Microsoft.Extensions.Configuration_](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/) which is used with one or more [configuration providers](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration#configuration-providers). It's commonly used for DI and Host configuration with the [_Microsoft.Extensions.Options_](https://www.nuget.org/packages/Microsoft.Extensions.Options/) library. 

The problem with this libraries is that all configuration and option is **read-only** which means that we can't update the configuration provided. That's the reason this library was made for, to make possible to update the file where the configuration was provided.

## Usage/Examples

Consider this Json file
```json
{
  "Settings": {
      "ConnectionString": "localhost:8080"
  }
}
```

The Usage:

```csharp
public class Settings
{
    public string ConnectionString { get; set; }
}

```

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AtoZ.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddWritableJsonFile<Settings>("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureWritable<Settings>("Settings", config);
var host = builder.build();

var writableSettings = host.Services.GetRequiredService<IWritableOptions<Settings>>();

Settings settings = writableSettings.Value;
settings.ConnectionString = "localhost:3050";

writableSettings.SaveChanges(settings);

```