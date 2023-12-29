using Alvz.Data.Extensions.Repository;
using System.Data;

namespace Alvz.Data.Extensions;

internal sealed class DatabaseManager : IDatabaseManager
{
    private readonly IMonitoredDbConnection _connection;
    private readonly RepositoryProvider _repositoryProvider;

    public DatabaseManager(IMonitoredDbConnection dbConnection, RepositoryProvider repositoryProvider)
    {
        _connection = dbConnection;
        _repositoryProvider = repositoryProvider;
    }

    public IDbConnection OpenConnection()
    {
        _connection.Open();
        return _connection;
    }

    public IDbTransaction BeginTransaction()
    {
        return _connection.BeginTransaction();
    }

    /// <summary>
    /// Abre conexão e inicia uma nova transação com o banco de dados.
    /// </summary>
    /// <returns></returns>
    public IDbConnection StartDatabaseOperation()
    {
        OpenConnection();
        BeginTransaction();
        return _connection;
    }

    public void Commit()
    {
        ArgumentNullException.ThrowIfNull(_connection.CurrentTransaction, nameof(_connection.CurrentTransaction));

        _connection.CurrentTransaction?.Commit();
    }

    public void CommitAsync()
    {
        ArgumentNullException.ThrowIfNull(_connection.CurrentTransaction, nameof(_connection.CurrentTransaction));

        _connection.CurrentTransaction?.CommitAsync();
    }

    public TRepository GetRepository<TRepository>()
        where TRepository : IRepository
    {
        return _repositoryProvider.GetRepository<TRepository>();
    }
}
