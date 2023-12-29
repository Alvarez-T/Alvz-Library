namespace Alvz.Extensions.Configuration.Exceptions;

internal class ConfigurationProviderNotFounded : Exception
{
    public ConfigurationProviderNotFounded(Type type)
        : base($"Não foi encontrado ConfigurationProvider para o tipo \"{type.Name}\". Certifique-se de tê-lo registrado antes de usar")
    {

    }
}
