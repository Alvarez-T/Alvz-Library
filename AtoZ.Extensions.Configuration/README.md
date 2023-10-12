
# A - Z Extensions Configuration

This library has some extensions targeting the [_Microsoft.Extensions.Configuration_](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/) which is used with one or more [configuration providers](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration#configuration-providers). It's commonly used for DI and Host configuration with the [_Microsoft.Extensions.Options_](https://www.nuget.org/packages/Microsoft.Extensions.Options/) library. 

The problem with this libraries is that all configuration and option is **read-only** which means that we can't update the configuration provided. That's the reason this library was made for, to make possible to update the file where the configuration was provided.

