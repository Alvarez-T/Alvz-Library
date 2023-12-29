namespace Alvz.Data.Extensions.Structure;

public sealed class TableFactory
{
    public IEnumerable<ITableStructure> GetTables()
    {
        foreach (var type in typeof(TableFactory).Assembly.GetTypes())
        {
            if (typeof(ITableStructure).IsAssignableFrom(type) && !type.IsInterface)
                yield return (ITableStructure)Activator.CreateInstance(type)!;
        }
    }
}
