namespace Demo.Domain.Models;

public record User(
    UserId Id,
    string FirstName,
    string LastName,
    string Email,
    DateOnly? DateOfBirth
)
{
   
}

public record UserId(Guid Value);

