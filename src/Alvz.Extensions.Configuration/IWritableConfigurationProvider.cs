using Microsoft.Extensions.Configuration;

namespace Alvz.Extensions.Configuration
{
    public interface IWritableConfigurationProvider<T> : IConfigurationProvider
        where T : class
    {
        public void UpdateFile(T serializable);
    }
}