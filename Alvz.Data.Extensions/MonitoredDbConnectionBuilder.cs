using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data.Common;

namespace Alvz.Data.Extensions;

public class MonitoredDbConnectionBuilder : IMonitoredDbConnectionBuilder
{
    private IConnectionStringProvider? _connectionString;
    private string? _databasePath;
    private DbConnection? _connection;
    private IDbConnectionFailureHandler? _failureHandler;
    private readonly ILoggerFactory _logger;

    public MonitoredDbConnectionBuilder(ILoggerFactory logger)
    {
        _logger = logger;
    }

    public IMonitoredDbConnectionBuilder DefineConnectionString(IConnectionStringProvider connectionStringProvider)
    {
        _connectionString = connectionStringProvider;
        return this;
    }

    public IMonitoredDbConnectionBuilder DefineDatabasePath(string databasePath)
    {
        _databasePath = databasePath;
        return this;
    }

    public IMonitoredDbConnectionBuilder DefineConnection(DbConnection dbConnection)
    {
        _connection = dbConnection;
        return this;
    }

    public IMonitoredDbConnectionBuilder DefineConnectionFailureHandler(IDbConnectionFailureHandler failureHandler)
    {
        _failureHandler = failureHandler;
        return this;
    }

    public MonitoredDbConnection Build()
    {
        ArgumentNullException.ThrowIfNull(_connection, nameof(_connection));
        ArgumentNullException.ThrowIfNull(_connectionString, nameof(_connectionString));

        _connection.ConnectionString = _connectionString.ToString();
        var database = new MonitoredDbConnection(_connection, _logger);
        return database;
    }
}
