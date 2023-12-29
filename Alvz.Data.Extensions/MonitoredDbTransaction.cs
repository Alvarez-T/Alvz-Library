using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Alvz.Data.Extensions;

public sealed class MonitoredDbTransaction : DbTransaction
{
    private readonly DbTransaction _transaction;
    private readonly ILogger<MonitoredDbTransaction> _logger;

    public MonitoredDbTransaction(DbTransaction dbTransaction, ILogger<MonitoredDbTransaction> logger)
    {
        _transaction = dbTransaction;
        _logger = logger;
        IdSession = Guid.NewGuid();
    }

    public override IsolationLevel IsolationLevel => _transaction.IsolationLevel;
    protected override DbConnection? DbConnection => _transaction.Connection;
    public Guid IdSession { get; }

    public override void Commit()
    {
        try
        {
            _transaction.Commit();
            _logger.LogInformation($"Commit efetuado com sucesso. [{IdSession}]");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Falha ao efetuar commit da operação. [{IdSession}]", ex);
            throw;
        }
    }

    public override void Rollback()
    {
        try
        {
            _transaction.Rollback();
            _logger.LogInformation($"Rollback efetuado com sucesso. [{IdSession}]");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Falha ao efetuar rollback da operação. [{IdSession}]", ex);
            throw;
        }
    }
}
