using Alvz.Data.Extensions.Exceptions;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Alvz.Data.Extensions;

public sealed class MonitoredDbConnection : DbConnection, IMonitoredDbConnection
{
    private readonly DbConnection _connection;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<MonitoredDbConnection> _logger;

    public MonitoredDbConnection(DbConnection dbConnection, ILoggerFactory loggerFactory)
    {
        _connection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _loggerFactory = loggerFactory;
        _connection = dbConnection;

        _logger = _loggerFactory.CreateLogger<MonitoredDbConnection>();
    }

    public override string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }
    public override string Database => _connection.ConnectionString;
    public override string DataSource => _connection.DataSource;
    public override string ServerVersion => _connection.ServerVersion;
    public override ConnectionState State => _connection.State;

    public MonitoredDbTransaction? CurrentTransaction { get; private set; }

    public Guid IdSession { get; private set; }

    public override void ChangeDatabase(string databaseName)
    {
        _connection.ChangeDatabase(databaseName);
    }

    public override void Close()
    {
        try
        {
            _connection.Close();
            _logger.LogInformation($"[{IdSession}] - Conexão com o banco de dados \"{DataSource}\" fechado.");
        }
        catch (DbException ex)
        {
            _logger.LogError($"[{IdSession}] - Falha ao fechar conexão com o banco de dados {DataSource}", ex);
            throw;
        }
    }

    public override async Task CloseAsync()
    {
        try
        {
            await _connection.CloseAsync();
            _logger.LogInformation($"[{IdSession}] - Conexão com o banco de dados {DataSource} fechado.");
        }
        catch (DbException ex)
        {
            _logger.LogError($"[{IdSession}] - Falha ao fechar conexão com o banco de dados {DataSource} assícrona", ex);
            throw;
        }
    }

    public override async Task OpenAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _connection.OpenAsync(cancellationToken);
            IdSession = Guid.NewGuid();
            _logger.LogInformation($"[{IdSession}] - Conexão com o banco de dados \"{DataSource}\" aberta");
        }
        catch (DbException ex)
        {
            _logger.LogError($"Falha ao conectar-se com o banco de dados \"{DataSource}\" assíncrono", ex);
            throw;
        }
    }

    public override void Open()
    {
        try
        {
            int tentativasDeReconexao = 3;
            do
            {
                try
                {
                    _connection.Open();
                    IdSession = Guid.NewGuid();
                    _logger.LogInformation($"[{IdSession}] - Conexão com o banco de dados \"{DataSource}\" aberta");
                }
                catch
                {
                    if (string.IsNullOrEmpty(ConnectionString))
                        throw new NotImplementedException();

                    Thread.Sleep(500);
                    tentativasDeReconexao--;
                }
            } while (tentativasDeReconexao > 0 && _connection.State != ConnectionState.Open);

            if (_connection.State != ConnectionState.Open)
                throw new ClosedConnectionException($"Número de tentativas para conexão com o banco de dados excedidas!");
        }
        catch (DbException ex)
        {
            if (ex is ClosedConnectionException)
                throw;

            _logger.LogError($"Falha ao conectar-se com o banco de dados {DataSource}", ex);
            throw new ClosedConnectionException($"Falha ao conectar-se com o banco de dados {DataSource}", ex);
        }
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        try
        {
            CurrentTransaction = new MonitoredDbTransaction(
                dbTransaction: _connection.BeginTransaction(isolationLevel),
                logger: _loggerFactory.CreateLogger<MonitoredDbTransaction>()
                );

            _logger.LogInformation($"[{CurrentTransaction.IdSession}] - Transação com nível de isolamento \"{Enum.GetName(typeof(IsolationLevel), isolationLevel)}\" iniciada [{CurrentTransaction.GetHashCode()}].");
            return CurrentTransaction;
        }
        catch (DbException ex)
        {
            _logger.LogError($"Falha ao iniciar transação com o banco de dados {DataSource}", ex);
            throw;
        }
    }

    protected override async ValueTask<DbTransaction> BeginDbTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        try
        {
            CurrentTransaction = new MonitoredDbTransaction(
                dbTransaction: await _connection.BeginTransactionAsync(isolationLevel),
                logger: _loggerFactory.CreateLogger<MonitoredDbTransaction>()
                );

            _logger.LogInformation($"[{CurrentTransaction.IdSession}] - Transação assíncrona com nível de isolamento \"{Enum.GetName(typeof(IsolationLevel), isolationLevel)}\" iniciada [{CurrentTransaction.GetHashCode()}].");
            return CurrentTransaction;
        }
        catch (DbException ex)
        {
            _logger.LogError($"Falha ao iniciar transação com o banco de dados {DataSource} assíncrona", ex);
            throw;
        }
    }

    protected override DbCommand CreateDbCommand()
    {
        try
        {
            MonitoredDbCommand? monitoredDbCommand = null;
            EnsureConnection(() =>
            {
                var cmd = _connection.CreateCommand();
                cmd.Transaction = CurrentTransaction;
                monitoredDbCommand = new MonitoredDbCommand(cmd, this, _loggerFactory.CreateLogger<MonitoredDbCommand>());
            });

            return monitoredDbCommand!;
        }
        catch (DbException ex)
        {
            _logger.LogError($"Falha ao criar comando SQL para o banco de dados {DataSource}", ex);
            throw;
        }
    }

    public void TestConnection()
    {
        _logger.LogDebug("Testando conexão com o banco de dados");
        Open();
        Thread.Sleep(100);
        Close();
    }

    private void EnsureConnection(Action method)
    {
        bool tempConnection = false;
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
            tempConnection = true;
        }

        method.Invoke();

        if (tempConnection)
            _connection.Close();
    }
}