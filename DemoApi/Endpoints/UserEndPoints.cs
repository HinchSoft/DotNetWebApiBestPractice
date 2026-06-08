using Asp.Versioning.Builder;
using Demo.Application.Handlers;
using DemoApi.Endpoints.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServiceManagement.CQRS;
using ServiceManagement.Results;

namespace DemoApi.Endpoints;

public class UserEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {

        app.MapPut("/users",
                async ([FromBody] NewUser newUser, ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new AddUserCommand.Command(
                    newUser.FirstName,
                    newUser.LastName,
                    newUser.Email,
                    DateOnly.FromDateTime(newUser.DatOfBirth!.Value)), cancellationToken))
            .HasApiVersion(1);

    }
}
