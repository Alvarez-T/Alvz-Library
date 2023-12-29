using Alvz.Data.Extensions.Repository;
using System.Data;

namespace Alvz.Data.Extensions;

public interface IDatabaseManager
{
    IDbTransaction BeginTransaction();
    void Commit();
    void CommitAsync();
    TRepository GetRepository<TRepository>() where TRepository : IRepository;
    IDbConnection OpenConnection();
    IDbConnection StartDatabaseOperation();
}