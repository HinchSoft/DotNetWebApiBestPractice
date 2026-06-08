using Domain.Results;


namespace ServiceManagement.CQRS;

public interface ISender
{  
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    
    Task Send(ICommand request, CancellationToken cancellationToken = default);
    Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default); 
    
    Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);
    Result<IAsyncEnumerable<TResponse>> Send<TResponse>(IAsyncEnumerableQuery<TResponse> request, CancellationToken cancellationToken = default); 
}