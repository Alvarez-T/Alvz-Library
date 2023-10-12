using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;

namespace AtoZ.Extensions.Configuration.Json
{
    public class JsonWritableConfigurationProvider<T> : JsonConfigurationProvider, IWritableConfigurationProvider<T>
        where T : class
    {
        public JsonWritableConfigurationProvider(JsonConfigurationSource source) : base(source)
        {
        }

        public void UpdateFile(T serializable)
        {
            var content = JsonSerializer.Serialize(
                serializable,
                new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });

            File.WriteAllText(this.Source.Path!, content);
        }
    }
}