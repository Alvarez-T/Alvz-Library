using System.Diagnostics.Contracts;

namespace Alvz.MediatR.Extensions
{
    public readonly struct Result
    {
        private readonly ResultState _state;
        public readonly string Message;

        public Result(ResultState status, string message = "")
        {
            _state = status;
            Message = message;
        }

        [Pure]
        public bool IsOk() => _state == ResultState.Ok;

        [Pure]
        public bool IsNotFounded() => _state == ResultState.NotFound;

        [Pure]
        public bool IsUnauthorized() => _state == ResultState.Unauthorized;

        [Pure]
        public bool IsCancelled() => _state == ResultState.Cancelled;

        [Pure]
        public bool IsBadRequest() => _state == ResultState.BadRequest;

        public TResult Match<TResult>(ResultState state, Func<TResult> a, Func<TResult> b) =>
            _state == state ? a() : b();

        public static Result Ok(string message = "") => new Result(ResultState.Ok, message);
        public static Result NotFounded(string message = "") => new Result(ResultState.NotFound, message);
        public static Result Unauthorized(string message = "") => new Result(ResultState.Unauthorized, message);
        public static Result Cancelled(string message = "") => new Result(ResultState.Cancelled, message);
    }

    public readonly struct QueryResult<T>
    {
        private readonly ResultState _state;
        public readonly T Value { get; }

        public readonly string Message { get; }

        public QueryResult(T value)
        {
            _state = ResultState.Ok;
            Value = value;
        }

        public QueryResult(ResultState status, string message = "")
        {
            _state = status;
            Value = default!;
        }

        [Pure]
        public static implicit operator QueryResult<T>(T? value)
        {
            return value is null
                ? new QueryResult<T>(ResultState.NotFound)
                : new QueryResult<T>(value);
        }

        [Pure]
        public bool IsOk() => _state == ResultState.Ok;

        [Pure]
        public bool IsNotFounded() => _state == ResultState.NotFound;

        [Pure]
        public bool IsUnauthorized() => _state == ResultState.Unauthorized;

        [Pure]
        public bool IsCancelled() => _state == ResultState.Cancelled;

        [Pure]
        public bool IsBadRequest() => _state == ResultState.BadRequest;

        public TResult Match<TResult>(ResultState state, Func<T, TResult> a, Func<T, TResult> b) =>
            _state == state ? a(Value) : b(Value);

    }
}
