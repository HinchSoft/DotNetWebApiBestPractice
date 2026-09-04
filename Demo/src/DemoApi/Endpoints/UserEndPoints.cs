using Asp.Versioning.Builder;
using DemoApplication.Handlers;
using DemoApi.Endpoints.Dtos;
using DemoApplication.Handlers.AddUser;
using Microsoft.AspNetCore.Mvc;
using RDH.Core.CQRS;


namespace DemoApi.Endpoints;

public class UserEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users",
            async ([FromBody] NewUser newUser, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(new AddUserCommand(
                newUser.FirstName,
                newUser.LastName,
                newUser.Email,
                DateOnly.FromDateTime(newUser.DatOfBirth!.Value)), cancellationToken));
    }
}
