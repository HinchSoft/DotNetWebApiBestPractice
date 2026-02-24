using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSServices.CQRS;

/// <summary>
/// Optional Handler base class this will try to executeAsync if execute is not implemented and try execute if
/// executeAsync is not implemented, without getting into an infinate loop if nether are implemented.
/// </summary>
/// <remarks> Some return types like <see cref="AsyncEnumerable"/> do not want to run asynchronously. 
/// </remarks>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class HandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private bool _redirected = false;

    public virtual TResponse Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        if(_redirected)
            throw new NotImplementedException();
        _redirected = true;
        return ExecuteAsync(request, cancellationToken).Result;
    }

    public virtual Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if(_redirected)
            throw new NotImplementedException();
        _redirected = true;
        return Task.FromResult(Execute(request, cancellationToken));
    }
}
