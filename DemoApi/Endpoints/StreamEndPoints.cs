using Api.Common.CQRS;
using CQRSServices.ApiResults;
using CQRSServices.CQRS;
using DemoApi.Handlers;
using DemoApi.Models;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

namespace DemoApi.Endpoints;

public class StreamEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("stream/users", (ISender sender, CancellationToken cancellationToken) =>
        {
            var res = sender.Send(new EnumerateUsers.Query(),cancellationToken);
            return res.HttpResult();
        });

        app.MapGet("serversentevents/users", (ISender sender, CancellationToken cancellationToken) =>
        {
            var res = sender.Send(new EnumerateUsers.Query(),cancellationToken);
            return res.HttpResultServerSentEvents();
        });
    }
}
