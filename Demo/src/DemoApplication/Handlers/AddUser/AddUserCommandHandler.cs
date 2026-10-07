using DemoApplication.Logging;
using DemoDomain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RDH.ApplicationLayer.Data;
using RDH.Core.CQRS;

namespace DemoApplication.Handlers.AddUser;

internal class AddUserCommandHandler(
    ILogger<AddUserCommandHandler> logger,
    IWriteRepository<User> userRepo,
    IUnitOfWork unitOfWork) : ICommandHandler<AddUserCommand>
{
    public async Task Execute(AddUserCommand request, CancellationToken cancellationToken = default)
    {
        if (await userRepo.AnyAsync(e =>
                e.Email == request.Email, cancellationToken))
        {
            UserLogging.EmailInUse(logger, request.FirstName, request.LastName, request.Email);
            ValidationException.ThrowConflict("Email already exists");
        }

        var user = new User(
            new UserId(Guid.NewGuid()),
            request.FirstName,
            request.LastName,
            request.Email,
            request.DateOfBirth
        );

        userRepo.Add(user);

        await unitOfWork.SaveChangesAsync();

        UserLogging.UserCreated(logger, user.Id, user.Email);
    }
}
