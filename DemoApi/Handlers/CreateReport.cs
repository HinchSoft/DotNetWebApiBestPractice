using CQRSServices.CQRS;
using CQRSServices.Results;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class CreateReport
{
    public class Handler(DataService dataService) : HandlerBase<Command, Result<Accepted>>
    {
        public override async Task<Result<Accepted>> ExecuteAsync(Command request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<Accepted>();
            var acc = await dataService.CreateReport(request.name, cancellationToken);
            res.AddValue(acc);  
            return res.Build();
        }
    }

    public record Command(string name):ICommand<Accepted>;

}

