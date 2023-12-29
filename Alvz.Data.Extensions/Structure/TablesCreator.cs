using Alvz.Data.Extensions;
using Alvz.Data.Extensions.Structure;

namespace Alvz.Data.Extensions.Structure;

public sealed class TablesCreator : ITableCreatorHandler
{
    private readonly List<ITableStructure> _tables;
    private readonly IDatabaseManager _database;

    private List<string> _alreadyExistedTables = new List<string>();

    public TablesCreator(IEnumerable<ITableStructure> tables, IDatabaseManager database)
    {
        _tables = tables.ToList();
        _database = database;
    }

    public bool CheckAllTablesExists()
    {
        foreach (var table in _tables)
        {
            if (VerifyIfTableExists(table.TableName))
                _alreadyExistedTables.Add(table.TableName);
        }

        return _alreadyExistedTables.Count == _tables.Count();
    }

    public void CreateTableStructure()
    {
        CreateTables();
        CreatePrimaryKeys();
        CreateForeignKeys();
        CreateGenerators();
        CreateTriggers();
        CreateViews();
    }

    private void CreateTables() => ForEachTable(table => table.CreateTable());
    private void CreatePrimaryKeys() => ForEachTable(table => table.CreatePrimaryKey());
    private void CreateForeignKeys() => ForEachTable(table => table.CreateForeignKeys());
    private void CreateGenerators() => ForEachTable(table => table.CreateGenerators());
    private void CreateTriggers() => ForEachTable(table => table.CreateTriggers());
    private void CreateViews() => ForEachTable(table => table.CreateViews());
    private void CreateProcedures() => ForEachTable(table => table.CreateProcedures());
    private void CreateFunctions() => ForEachTable(table => table.CreateFunctions());

    private void ForEachTable(Action<ITableStructure> createAction)
    {
        foreach (var table in _tables)
        {
            if (_alreadyExistedTables.Contains(table.TableName))
                continue;

            createAction(table);
        }
    }

    private bool VerifyIfTableExists(string tableName)
    {
        //TODO: Implementar
        throw new NotImplementedException();
        //string sql = $"SELECT 1 FROM RDB$RELATIONS WHERE RDB$RELATION_NAME = {tableName.ToSql().ToUpper()}";
        //return _database.Query(connection => connection.ExecuteScalar(sql)) is not null;
    }
}
