using CQRSServices.CQRS;
using CQRSServices.Results;
using CQRSServices.ServiceResponses;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class GetUser
{
    public class Handler(DataService dataService) : HandlerBase<Query, Result<User>>
    {
        public override async Task<Result<User>> ExecuteAsync(Query request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<User>();
            var user = await dataService.GetUserAsync(request.UserId,cancellationToken);
            if (user is not null)
                res.AddValue(user);
            else
                res.AddNotFound($"User with id {request.UserId} not found");
                
            return res.Build();
        }
    }

    public record Query(int UserId):IQuery<User>;
}

