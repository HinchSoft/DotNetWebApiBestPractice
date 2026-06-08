using System.Diagnostics;
using Domain.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceManagement.CQRS;

public class Sender(IServiceProvider provider) : ISender
{
    /// <inheritdoc/>
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        object handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task<TResponse>>(handlerType, handler, request, cancellationToken);
    }

    /// <inheritdoc/>
    public Task Send(ICommand request, CancellationToken cancellationToken = default)
    {
        Debug.Assert(request != null, nameof(request) + " != null");
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        var handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task>(handlerType, handler, request, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task<TResponse>>(handlerType, handler, request, cancellationToken);
    }


    /// <inheritdoc/>
    public Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        object handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task<TResponse>>(handlerType, handler, request, cancellationToken);
    }

    /// <inheritdoc/>
    public Result<IAsyncEnumerable<TResponse>> Send<TResponse>(IAsyncEnumerableQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IAsyncEnumerableQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        object handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Result<IAsyncEnumerable<TResponse>>>(handlerType, handler, request, cancellationToken);
    }

    private static T ExecuteMethod<T>(Type handlerType, object handler, params object[] args)
    {
        var method = handlerType.GetMethod("Execute");
        if (method == null) throw new InvalidOperationException($"Execute method not found on '{handlerType.Name}'");
        return (T)method.Invoke(handler, args)!;
    }
}
