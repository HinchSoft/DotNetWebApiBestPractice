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

public interface ICommand:IRequest<Result>;

public interface ICommand<TResponse>:IRequest<Result<TResponse>>;

public interface IQuery<TResponse>:IRequest<Result<TResponse>>;

public interface IRequestHandler<TRequest,TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
    TResponse Execute(TRequest request, CancellationToken cancellationToken = default);
}
