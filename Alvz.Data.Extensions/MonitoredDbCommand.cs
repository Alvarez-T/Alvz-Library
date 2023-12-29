using Alvz.Data.Extensions.Exceptions;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Alvz.Data.Extensions;

internal sealed class MonitoredDbCommand : DbCommand
{
    private DbCommand _command;
    private readonly ILogger<MonitoredDbCommand> _logger;

    public MonitoredDbCommand(DbCommand decoratedCommand, DbConnection dbConnection, ILogger<MonitoredDbCommand> logger)
    {
        ArgumentNullException.ThrowIfNull(dbConnection, nameof(dbConnection));

        if (dbConnection.State == ConnectionState.Closed)
            throw new ClosedConnectionException("A conexão deverá estar aberta para criar comando SQL");

        Connection = dbConnection;
        _command = decoratedCommand;
        _logger = logger;
    }

    public override string? CommandText
    {
        get => _command.CommandText;
        set => _command.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => _command.CommandTimeout;
        set => _command.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => _command.CommandType;
        set => _command.CommandType = value;
    }

    public override bool DesignTimeVisible
    {
        get => _command.DesignTimeVisible;
        set => _command.DesignTimeVisible = value;
    }

    protected override DbConnection DbConnection
    {
        get => _command.Connection!;
        set => _command.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => _command.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => _command.Transaction;
        set => _command.Transaction = value;
    }
    public override UpdateRowSource UpdatedRowSource { get => _command.UpdatedRowSource; set => _command.UpdatedRowSource = value; }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        try
        {
            _logger.LogDebug($"(ExecuteDbDataReader) Executando Query:\n{_command.CommandText}\n");
            return _command.ExecuteReader(behavior);
        }
        catch (DbException ex)
        {
            LogCommandError(ex);
            throw;
        }
    }

    protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug($"(ExecuteDbDataReaderAsync) Executando Query:\n{_command.CommandText}\n");
            return _command.ExecuteReaderAsync(behavior, cancellationToken);
        }
        catch (DbException ex)
        {
            LogCommandError(ex);
            throw;
        }
    }

    public override int ExecuteNonQuery()
    {
        try
        {
            int? commandResult = null;
            EnsureTransaction(() =>
            {
                _logger.LogDebug($"(ExecuteNonQuery) Executando comando SQL:\n{_command.CommandText}\n");
                commandResult = _command.ExecuteNonQuery();
            });

            return commandResult!.Value;
        }
        catch (DbException ex)
        {
            LogCommandError(ex);
            throw;
        }
    }

    public async override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
    {
        try
        {
            int? commandResult = null;
            await EnsureTranscationAsync(async () =>
            {
                _logger.LogDebug($"(ExecuteNonQueryAsync) Executando comando SQL:\n{_command.CommandText}\n");
                commandResult = await _command.ExecuteNonQueryAsync(cancellationToken);
            });

            return commandResult!.Value;
        }
        catch (DbException ex)
        {
            LogCommandError(ex);
            throw;
        }
    }

    public override object? ExecuteScalar()
    {
        try
        {
            _logger.LogDebug($"(ExecuteScalar) Executando comando SQL:\n{_command.CommandText}\n");
            return _command.ExecuteScalar();
        }
        catch (DbException ex)
        {
            LogCommandError(ex);
            throw;
        }
    }

    public override Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug($"(ExecuteScalarAsync) Executando comando SQL:\n{_command.CommandText}\n");
            return _command.ExecuteScalarAsync(cancellationToken);
        }
        catch (DbException ex)
        {
            LogCommandError(ex);
            throw;
        }
    }

    public override void Cancel()
    {
        try
        {
            _logger.LogDebug($"(Cancel) Cancelando comando SQL:\n{_command.CommandText}\n");
            _command.Cancel();
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, $"Falha ao cancelar comando SQL:\n{CommandText}\n");
            throw;
        }
    }

    public override void Prepare()
    {
        try
        {
            _logger.LogDebug($"(Prepare) Preparando comando SQL:\n{_command.CommandText}\n");
            _command.Prepare();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Falha ao preparar comando SQL:\n{CommandText}\n");
            throw;
        }
    }

    private void LogCommandError(Exception ex) => _logger.LogError(ex, $"Falha ao executar comando SQL: \n{CommandText}\n");

    protected override DbParameter CreateDbParameter()
    {
        return _command.CreateParameter();
    }

    private void EnsureTransaction(Action method)
    {
        bool tempTransaction = false;
        if (Transaction is null)
        {
            Transaction = _command.Connection!.BeginTransaction();
            tempTransaction = true;
        }

        method.Invoke();

        if (tempTransaction)
        {
            Transaction.Commit();
            Transaction.Dispose();
            Transaction = null;
        }
    }

    private async Task EnsureTranscationAsync(Func<Task> method)
    {
        bool tempTransaction = false;
        if (Transaction is null)
        {
            Transaction = await _command.Connection!.BeginTransactionAsync();
            tempTransaction = true;
        }

        await method.Invoke();

        if (tempTransaction)
        {
            Transaction.Commit();
            Transaction.Dispose();
            Transaction = null;
        }
    }
}
