using CQRSServices.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Common.CQRS;

public class Sender(IServiceProvider provider) : ISender
{
    /// <inheritdoc/>
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetRequiredService(handlerType);
        return handler.ExecuteAsync((dynamic)request, cancellationToken);
    }

    /// <inheritdoc/>
    public TResponse Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetRequiredService(handlerType);
        return handler.Execute((dynamic)request, cancellationToken);
    }
}
