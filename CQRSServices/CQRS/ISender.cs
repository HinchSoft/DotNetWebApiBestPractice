namespace CQRSServices.CQRS;

public interface ISender
{
    TResponse Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
