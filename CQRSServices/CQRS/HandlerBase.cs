using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSServices.CQRS;

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
