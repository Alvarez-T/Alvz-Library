using System.Data;

namespace Alvz.Data.Extensions
{
    public interface IDbConnectionFailureHandler
    {
        bool CanHandle(IDbConnection dbConnection);
        void Handle();
    }
}
