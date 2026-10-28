# RDH.ApplicationLayer

`RDH.ApplicationLayer` defines application-level abstractions, repository contracts, and unit-of-work patterns. It forms the boundary between business use cases / CQRS handlers and concrete persistence infrastructure (such as Entity Framework Core).

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package RDH.ApplicationLayer
```

Or via the Package Manager Console:

```powershell
Install-Package RDH.ApplicationLayer
```

---

## Key Features

- **Persistence Boundaries:** Clean abstractions for data access (`IReadRepository<TEntity>`, `IWriteRepository<TEntity>`) keeping domain and application handlers decoupled from ORMs.
- **Unit of Work:** `IUnitOfWork` contract providing atomic transactional save points (`SaveChangesAsync`).
- **Pagination & Utilities:** Contracts for paginated API results (`Pagination`, `IApiUtilities`).
- **Inward Dependency:** References `RDH.Core` but has zero external dependencies on EF Core, ASP.NET Core, or database drivers.

---

## Sample Code

### 1. Consuming Repositories in a Command Handler

In your CQRS handler, inject `IWriteRepository<TEntity>` and `IUnitOfWork` to persist entities without referencing concrete database contexts:

```csharp
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
        // Check for existing records
        if (await userRepo.AnyAsync(e => e.Email == request.Email, cancellationToken))
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

        // Add entity to repository
        userRepo.Add(user);

        // Commit transaction through unit of work
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
```

### 2. Querying via `IReadRepository<TEntity>`

```csharp
using RDH.ApplicationLayer.Data;
using RDH.Core.CQRS;

public class GetUserByIdQueryHandler(IReadRepository<User> userRepo) 
    : IQueryHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Execute(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByIdAsync(request.Id, cancellationToken);
        return user is null ? null : new UserDto(user.Id.Value, user.FirstName, user.LastName, user.Email);
    }
}
```
