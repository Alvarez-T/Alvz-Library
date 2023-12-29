using MediatR;

namespace Alvz.MediatR.Extensions
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {

    }

    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, QueryResult<TResponse>>
        where TQuery : IQuery<TResponse>
    {

    }
}
