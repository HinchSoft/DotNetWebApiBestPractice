using Domain;

namespace DemoDomain.Models;

public record User(
    UserId Id,
    string FirstName,
    string LastName,
    string Email,
    DateOnly? DateOfBirth
);

public record UserId(Guid Value):EntityId(Value);
