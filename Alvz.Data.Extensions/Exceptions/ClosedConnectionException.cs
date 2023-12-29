using System.Data.Common;

namespace Alvz.Data.Extensions.Exceptions;

public class ClosedConnectionException : DbException
{
    public ClosedConnectionException() : base()
    {

    }
    public ClosedConnectionException(string message, Exception? innerException = null) : base(message, innerException)
    {

    }


}
