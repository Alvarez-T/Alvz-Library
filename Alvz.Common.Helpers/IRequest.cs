using MediatR;

namespace Alvz.MediatR.Extensions
{
    public interface ICommand : IRequest<Result>
    {

    }

    public interface IQuery<T> : IRequest<QueryResult<T>>
    {
    }
}
