namespace Alvz.Data.Extensions.Structure;

public interface ITableCreatorHandler
{
    bool CheckAllTablesExists();
    void CreateTableStructure();
}
