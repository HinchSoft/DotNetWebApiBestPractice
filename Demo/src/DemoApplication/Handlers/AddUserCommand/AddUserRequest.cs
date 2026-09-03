using RDH.Core.CQRS;

namespace DemoApplication.Handlers.AddUserCommand;

public record AddUserRequest(string FirstName, string LastName, string Email, DateOnly DateOfBirth) : ICommand;
