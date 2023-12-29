using System.Data.Common;

namespace Alvz.Data.Extensions
{
    public interface IMonitoredDbConnectionBuilder
    {
        MonitoredDbConnection Build();
        IMonitoredDbConnectionBuilder DefineConnection(DbConnection dbConnection);
        IMonitoredDbConnectionBuilder DefineConnectionString(IConnectionStringProvider connectionStringProvider);
        IMonitoredDbConnectionBuilder DefineConnectionFailureHandler(IDbConnectionFailureHandler failureHandler);
        IMonitoredDbConnectionBuilder DefineDatabasePath(string databasePath);
    }
}