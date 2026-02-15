using CQRSServices.CQRS;
using CQRSServices.Results;
using CQRSServices.ServiceResponses;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class UpdateUser
{
    public class Handler(DataService dataService) : HandlerBase<Command, Result<User>>
    {
        public override async Task<Result<User>> ExecuteAsync(Command request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<User>();
            var user = await dataService.GetUserAsync(request.Id, cancellationToken);
            if(user is null)
                return res.AddNotFound($"User Id {request.Id} not found").Build();
              
            return res.Build();
        }
    }

    public record Command(int Id, string Name):ICommand<User>;
}

