using Microsoft.Extensions.Options;

namespace Alvz.Extensions.Configuration;

public interface IWritableOptions<T> : IOptionsSnapshot<T> where T : class
{
    void SaveChanges(T optionChanged);
}
