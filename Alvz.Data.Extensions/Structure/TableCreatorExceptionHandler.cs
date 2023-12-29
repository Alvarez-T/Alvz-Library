//using Microsoft.Extensions.Logging;

//namespace Alvz.Data.Extensions.Structure;

//public class TableCreatorExceptionHandler : ITableStructure
//{
//    private readonly ITableStructure _table;
//    private readonly ILogger<ITableStructure> _logger;

//    public TableCreatorExceptionHandler(ITableStructure table, ILogger<ITableStructure> logger)
//    {
//        _table = table;
//        _logger = logger;
//    }

//    public string TableName => _table.TableName;

//    public void CreateTable()
//    {
//        try
//        {
//            _logger.LogInformation($"Criando tabela: {TableName}");

//            _table.CreateTable();

//            _logger.LogInformation($"Tabela {TableName} criada com sucesso");
//        }
//        catch (Exception ex)
//        {
//            throw new TableCreationException(TableName, ex);
//        }
//    }

//    public void CreatePrimaryKey()
//    {
//        try
//        {
//            _logger.LogInformation($"Criando Primary Key da tabela {TableName}");

//            _table.CreatePrimaryKey();

//            _logger.LogInformation($"Primary Key da tabela {TableName} criada com sucesso");
//        }
//        catch (Exception ex)
//        {
//            throw new TableCreationException(TableName, ex,
//                message: "Ocorreu um erro na criação do Primary Key");
//        }
//    }

//    public void CreateForeignKeys()
//    {
//        try
//        {
//            _logger.LogInformation($"Criando Foreign Keys da tabela {TableName}");

//            _table.CreateForeignKeys();

//            _logger.LogInformation($"Foreign Keys da tabela criada com sucesso {TableName}");
//        }
//        catch (Exception ex)
//        {
//            throw new TableCreationException(TableName, ex,
//                message: "Ocrreu um erro na criação dos Foreign Keys");
//        }
//    }

//    public void CreateIndexers()
//    {
//        try
//        {
//            _logger.LogInformation($"Criando Indexers da tabela {TableName}");

//            _table.CreateIndexers();

//            _logger.LogInformation($"Indexers da tabela {TableName} criado com sucesso");
//        }
//        catch (Exception ex)
//        {
//            throw new TableCreationException(TableName, ex,
//                message: "Ocorreu um erro na criação de Indexers");
//        }
//    }

//    public void CreateGenerators()
//    {
//        try
//        {
//            _logger.LogInformation($"Criando Generators da tabela {TableName}");

//            _table.CreateGenerators();

//            _logger.LogInformation($"Generators da tabela {TableName} criado com sucesso");
//        }
//        catch (Exception ex)
//        {
//            throw new TableCreationException(TableName, ex,
//                message: "Ocorreu um erro na criação dos Generators");
//        }
//    }

//    public void CreateTriggers()
//    {
//        try
//        {
//            _logger.LogInformation($"Criando Triggers da tabela {TableName}");

//            _table.CreateTriggers();

//            _logger.LogInformation($"Triggers da tabela {TableName} criado com sucesso");
//        }
//        catch (Exception ex)
//        {
//            throw new TableCreationException(TableName, ex,
//                message: "Ocorreu um erro na criação de Triggers");
//        }
//    }
//}
