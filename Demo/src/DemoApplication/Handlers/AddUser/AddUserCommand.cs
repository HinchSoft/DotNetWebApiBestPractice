using RDH.Core.CQRS;

namespace DemoApplication.Handlers.AddUser;

public record AddUserCommand(string FirstName, string LastName, string Email, DateOnly DateOfBirth) : ICommand;
