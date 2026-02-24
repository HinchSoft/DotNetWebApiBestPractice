using CQRSServices.Results;

namespace CQRSServices.CQRS;

/// <summary>
/// General MediatR style IRequest interface
/// </summary>
/// <remarks>
/// This is the base to all CQRS requests
/// </remarks>
/// <typeparam name="TResponse"></typeparam>
public interface IRequest<TResponse>;

/// <summary>
/// Represents input data that can be executed by a handler, it has no return value but uses a <see cref="Result"/> to return status.
/// </summary>
public interface ICommand:IRequest<Result>;

/// <summary>
/// Represents a command that returns a result of type TResponse when executed.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
public interface ICommand<TResponse>:IRequest<Result<TResponse>>;

/// <summary>
/// Represents a query that returns a result of type TResponse. The handler should not make any changes to data.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
public interface IQuery<TResponse>:IRequest<Result<TResponse>>;

/// <summary>
/// Defines a contract for handling a request and returning a response, supporting both asynchronous and synchronous
/// execution patterns.
/// </summary>
/// <remarks>Implementations of this interface should provide the logic for processing requests and generating
/// responses. Use ExecuteAsync for non-blocking operations and Execute for synchronous scenarios. Both methods accept a
/// cancellation token to support cooperative cancellation.</remarks>
/// <typeparam name="TRequest">The type of the request to handle. Must implement the IRequest<TResponse> interface and defines the structure of the
/// request data.</typeparam>
/// <typeparam name="TResponse">The type of the response returned after processing the request.</typeparam>
public interface IRequestHandler<TRequest,TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
    TResponse Execute(TRequest request, CancellationToken cancellationToken = default);
}
