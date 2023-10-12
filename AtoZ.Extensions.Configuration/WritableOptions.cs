using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AtoZ.Extensions.Configuration;

internal class WritableOptions<T> : IWritableOptions<T> where T : class
{
    private readonly IOptionsMonitor<T> _options;
    private readonly IWritableConfigurationProvider<T> _writableFileConfigurationProvider;

    public WritableOptions(IOptionsMonitor<T> optionsMonitor, IWritableConfigurationProvider<T> writableFileConfigurationProvider)
    {
        _options = optionsMonitor;
        _writableFileConfigurationProvider = writableFileConfigurationProvider;
    }

    public T Value => _options.CurrentValue;
    public T Get(string? name) => _options.Get(name);
    public void SaveChanges(T optionChanged) => _writableFileConfigurationProvider.UpdateFile(optionChanged);
}
