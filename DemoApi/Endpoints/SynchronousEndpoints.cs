
using CQRSServices.ApiResults;
using CQRSServices.CQRS;
using DemoApi.Endpoints.Dtos;
using DemoApi.Handlers;
using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Endpoints;

public class SynchronousEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("sync/users", async (ISender sender) =>
        {
            var res = await sender.SendAsync(new ListUsers.Query());
            return res.HttpResult();
        });
        app.MapGet("sync/users/{id:int}", async (int id, ISender sender) =>
        {
            var res = await sender.SendAsync(new GetUser.Query(id));
            return res.HttpResult();
        });
        app.MapPut("sync/users", async ([FromBody] UserDetail userDetail, ISender sender) =>
        {
            var res = await sender.SendAsync(new AddUser.Command(userDetail.Name));
            return res.HttpResultCreated(v=>$"sync/users/{v.Id}");
        });
        app.MapPost("sync/users/{id:int}", async (int id, [FromBody] UserDetail userDetail, ISender sender) =>
        {
            var res = await sender.SendAsync(new UpdateUser.Command(id, userDetail.Name));
            return res.HttpResult();
        });
        app.MapGet("sync/Report/{id}", async (string id, ISender sender, IHttpResultService resultService) =>
        {
            var res = await sender.SendAsync(new CreateReport.Command(id));

            return res.HttpResultAccepted(v=>resultService.GenerateLocationFromPath($"sync/Status/{v.Id}"), (v,l) = new AcceptStatus(v.ReportId)));
        });


    }
}

public record AcceptStatus(string Id);
