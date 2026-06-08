using Demo.Domain.Data;
using Demo.Domain.Models;
using Domain;
using ServiceManagement.CQRS;
using Domain.Results;
using FluentValidation;

namespace Demo.Application.Handlers;

public static class AddUserCommand
{
    internal class Handler(IUnitOfWork unitOfWork, 
        IWriteRepository<User> userRepo,
        IValidator<User> userValidator) : ICommandHandler<Command>
    {
        public async Task Execute(Command request, CancellationToken cancellationToken = default)
        {
            if(await userRepo.AnyAsync(e=> 
                   request.Email.Equals(e.Email,StringComparison.InvariantCultureIgnoreCase), cancellationToken))
            {
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
        }
    }

    public record Command(string FirstName, string LastName, string Email, DateOnly DateOfBirth) : ICommand;
}
