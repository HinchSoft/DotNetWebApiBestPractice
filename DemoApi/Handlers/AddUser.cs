using CQRSServices.CQRS;
using CQRSServices.Results;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class AddUser
{
    public class Handler(DataService dataService) : HandlerBase<Command, Result<User>>
    {
        public override async Task<Result<User>> ExecuteAsync(Command request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<User>();
            if (await dataService.GetUserByNameAsync(request.Name, cancellationToken) is not null)
                return res.AddConflict($"User with name {request.Name} already exists").Build();

            var user = await dataService.AddUserAsync(request.Name,cancellationToken);
            res.AddValue(user);
               
            return res.Build();
        }
    }

    public record Command(string Name):ICommand<User>;
}

