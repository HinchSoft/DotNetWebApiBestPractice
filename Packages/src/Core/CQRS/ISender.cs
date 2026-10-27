using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RDH.Core.Results;

namespace RDH.Core.CQRS;

public interface ISender
{
    /// <summary>
    /// Locate and execute a handler that takes a IRequest[TResponse] and returns a TResponse
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Locate and execute a handler that takes a ICommand and has no response
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Send(ICommand request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Locate and execute a handler that takes a ICommand[TResponse] and returns a TResponse
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Locate and execute a handler that takes a IQuery[TResponse] and returns a TResponse
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Locate and execute a handler that takes a IAsyncEnumerableQuery[TResponse] and returns a IAsyncEnumerable[TResponse]
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Result<IAsyncEnumerable<TResponse>> Send<TResponse>(IAsyncEnumerableQuery<TResponse> request,
        CancellationToken cancellationToken = default);
}