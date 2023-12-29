namespace Alvz.Data.Extensions.Structure
{
    public interface ITableStructure
    {
        public string TableName { get; }
        string CreateTable();
        string? CreateForeignKeys() => null;
        string? CreateIndexers() => null;
        string? CreatePrimaryKey() => null;
        string? CreateGenerators() => null;
        string? CreateTriggers() => null;
        string? CreateViews() => null;
        string? CreateProcedures() => null;
        string? CreateFunctions() => null;
    }
}
