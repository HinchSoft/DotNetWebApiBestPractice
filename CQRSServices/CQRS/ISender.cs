namespace CQRSServices.CQRS;

public interface ISender
{
    /// <summary>
    /// Find a handler and execute it synchronously
    /// </summary>
    TResponse Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find a handler and execute it asynchronously
    /// </summary>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
