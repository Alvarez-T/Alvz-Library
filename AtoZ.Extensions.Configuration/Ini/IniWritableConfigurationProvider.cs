using Microsoft.Extensions.Configuration.Ini;

namespace AtoZ.Extensions.Configuration.Ini
{

    public class IniWritableConfigurationProvider<T> : IniConfigurationProvider, IWritableConfigurationProvider<T>
        where T : class
    {
        public IniWritableConfigurationProvider(IniConfigurationSource source) : base(source)
        {
        }

        public void UpdateFile(T serializable)
        {

        }
    }
}