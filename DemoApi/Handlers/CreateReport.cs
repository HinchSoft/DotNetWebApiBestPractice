using CQRSServices.CQRS;
using CQRSServices.Results;
using CQRSServices.ServiceResponses;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Handlers;

public class CreateReport
{
    public class Handler(DataService dataService) : HandlerBase<Command, Result<Response>>
    {
        public override async Task<Result<Response>> ExecuteAsync(Command request, CancellationToken cancellationToken = default)
        {
            var res = new ResultBuilder<Response>();
            //var user = await dataService.GetUserAsync(request.Id, cancellationToken);
            res.AddValue(new Response(request.Id, 20));  
            return res.Build();
        }
    }

    public record Command(string Id):ICommand<Response>;

    public record Response(string ReportId,int Id);
}

