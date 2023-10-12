using Microsoft.Extensions.Options;

namespace AtoZ.Extensions.Configuration;

public interface IWritableOptions<T> : IOptionsSnapshot<T> where T : class
{
    void SaveChanges(T optionChanged);
}
