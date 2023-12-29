using System.Data;

namespace Alvz.Data.Extensions
{
    public interface IMonitoredDbConnection : IDbConnection
    {
        //string DatabasePath { get; }
        MonitoredDbTransaction? CurrentTransaction { get; }

        //   void DisposeConnection();
        void TestConnection();
    }
}