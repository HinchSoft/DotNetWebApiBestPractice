using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RDH.Core.Results;

namespace RDH.Core.CQRS;

public sealed class Sender(IServiceProvider provider) : ISender
{
    /// <inheritdoc/>
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        // Identify and create handler instance
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        object handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task<TResponse>>(handlerType, handler, request, cancellationToken);
    }

    /// <inheritdoc/>
    public Task Send(ICommand request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        // Identify and create handler instance
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        var handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task>(handlerType, handler, request, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        // Identify and create handler instance
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task<TResponse>>(handlerType, handler, request, cancellationToken);
    }


    /// <inheritdoc/>
    public Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        // Identify and create handler instance
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Task<TResponse>>(handlerType, handler, request, cancellationToken);
    }

    /// <inheritdoc/>
    public Result<IAsyncEnumerable<TResponse>>
        Send<TResponse>(IAsyncEnumerableQuery<TResponse> request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);
        // Identify and create handler instance
        var handlerType = typeof(IAsyncEnumerableQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetRequiredService(handlerType);
        return ExecuteMethod<Result<IAsyncEnumerable<TResponse>>>(handlerType, handler, request, cancellationToken);
    }

    private static T ExecuteMethod<T>(Type handlerType, object handler, params object[] args)
    {
        var method = handlerType.GetMethod("Execute");
        if (method == null) throw new InvalidOperationException($"Execute method not found on '{handlerType.Name}'");
        return (T)method.Invoke(handler, args)!;
    }

    private void ValidateRequest<TRequest>(TRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        // look for FluentValidator for this request type
        var reqType = request.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(reqType);
        var validator = provider.GetService(validatorType);
        if (validator == null) return;
        var method = validatorType.GetMethod("ValidateAndThrow");
        if (method == null) return;
        _ = method.Invoke(validator, [request]);
    }
}
