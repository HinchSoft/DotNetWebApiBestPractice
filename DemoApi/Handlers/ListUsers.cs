using CQRSServices.CQRS;
using CQRSServices.Results;
using CQRSServices.ServiceResponses;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class ListUsers
{
    public class Handler(DataService dataService) : HandlerBase<Query, Result<IEnumerable<User>>>
    {
        public override async Task<Result<IEnumerable<User>>> ExecuteAsync(Query request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<IEnumerable<User>>();
            var users = await dataService.GetUsersAsync(cancellationToken);
            res.AddValue(users);

            return res.Build();
        }

        public Result<IAsyncEnumerable<User>> Handle(Query query, CancellationToken cancellationToken)
        {
            var res = new ResultBuilder<IAsyncEnumerable<User>>();
            res.AddValue(dataService.GetUsersSlowAsync(cancellationToken));

            return res.Build();
        }
    }

    public record Query() : IQuery<IEnumerable<User>>;
}

