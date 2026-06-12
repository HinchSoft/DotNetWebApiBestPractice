using DemoApplication.Logging;
using DemoDomain.Models;
using Domain;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ServiceManagement.CQRS;
using ServiceManagement.Data;

namespace DemoApplication.Handlers;

public static class AddUserCommand
{
    internal class Handler(
        ILogger<Handler> logger,
        IUnitOfWork unitOfWork,
        IWriteRepository<User> userRepo,
        IValidator<User> userValidator) : ICommandHandler<Command>
    {
        public async Task Execute(Command request, CancellationToken cancellationToken = default)
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

            await userValidator.ValidateAndThrowAsync(user, cancellationToken);

            userRepo.Add(user);

            await unitOfWork.SaveChangesAsync();

            UserLogging.UserCreated(logger, user.Id, user.Email);
        }
    }

    public record Command(string FirstName, string LastName, string Email, DateOnly DateOfBirth) : ICommand;
}
