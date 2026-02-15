using CQRSServices.CQRS;
using CQRSServices.Results;
using CQRSServices.ServiceResponses;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class EnumerateUsers
{
    public class Handler(DataService dataService) : HandlerBase<Query, Result<IAsyncEnumerable<User>>>
    {

        public override Result<IAsyncEnumerable<User>> Execute(Query request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<IAsyncEnumerable<User>>();
            res.AddValue(dataService.GetUsersSlowAsync(cancellationToken));

            return res.Build();
        }
    }

    public record Query() : IQuery<IAsyncEnumerable<User>>;
}

