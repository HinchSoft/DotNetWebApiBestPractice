using Domain.Results;


namespace ServiceManagement.CQRS;

/// <summary>
/// General MediatR style IRequest interface
/// </summary>
/// <remarks>
/// This is the base to all CQRS requests
/// </remarks>
/// <typeparam name="TResponse"></typeparam>
public interface IRequest<TResponse>;

/// <summary>
/// Represents input data that can be executed by a handler, it has no return value.
/// </summary>
public interface ICommand;

/// <summary>
/// Represents a command that returns a result of type TResponse when executed.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
public interface ICommand<TResponse>:IRequest<TResponse>;

/// <summary>
/// Represents a query that returns a result of type TResponse. The handler should not make any changes to data.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
public interface IQuery<TResponse>:IRequest<TResponse>;

/// <summary>
/// Represents a query that returns a AsyncEnumerable of type TResponse. The handler should not make any changes to data.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
public interface IAsyncEnumerableQuery<TResponse>:IRequest<IAsyncEnumerable<TResponse>>;

/// <summary>
/// Defines a contract for handling a request and returning a response
/// </summary>
public interface IRequestHandler<TRequest,TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a contract for handling a command with no return type
/// </summary>
public interface ICommandHandler<TRequest>
    where TRequest : ICommand
{
    Task Execute(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a contract for handling a command and returning a Result of type TResponse
/// </summary>
public interface ICommandHandler<in TRequest,TResponse>
    where TRequest : ICommand<TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a contract for handling a query and returning a response, the implementation should not
/// change the data.
/// </summary>
public interface IQueryHandler<in TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a contract for handling a query and returning an async enumerable of type TResponse, the implenetation should not
/// change the data.
/// </summary>
public interface IAsyncEnumerableQueryHandler<TRequest, TResponse>
    where TRequest : IAsyncEnumerableQuery<TResponse>
{
    Result<IAsyncEnumerable<TResponse>> Execute(TRequest request, CancellationToken cancellationToken = default);
}
