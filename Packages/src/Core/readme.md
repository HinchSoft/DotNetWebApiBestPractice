# RDH.Core

`RDH.Core` provides foundational, framework-agnostic building blocks for .NET applications adhering to Clean Architecture and CQRS patterns. It contains CQRS abstractions, pipeline dispatching, strongly typed identifiers, result wrappers, and PII data classifications.

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package RDH.Core
```

Or via the Package Manager Console:

```powershell
Install-Package RDH.Core
```

---

## Key Features

- **CQRS Abstractions:** Core contracts for `ICommand`, `ICommand<TResponse>`, `IQuery<TResponse>`, and `IRequestHandler`.
- **In-Memory Dispatcher:** `ISender` / `Sender` pipeline for executing requests with built-in FluentValidation validation before execution.
- **Strongly Typed Identifiers:** Abstract `EntityId` record wrapping `Guid` to prevent accidental entity ID mixing.
- **Result Models:** `Result` and `Result<T>` records with fluent `ResultBuilder` support for handling outcomes without relying on exceptions for control flow.
- **PII Compliance Attributes:** `[PersonalPII]` and `[PrivatePII]` data classification attributes for structured, GDPR-conscious logging.
- **FluentValidation Integration:** Helper methods for attaching HTTP status metadata to validation failures (e.g. `ValidationException.ThrowConflict`).

---

## Sample Code

### 1. Registering CQRS in Dependency Injection

Discover and register all CQRS handlers automatically from an assembly:

```csharp
using Microsoft.Extensions.DependencyInjection;
using RDH.Core.CQRS;

// Registers ISender and all IRequestHandler/ICommandHandler/IQueryHandler implementations
builder.Services.AddCQRS(typeof(AddUserCommand).Assembly);
```

### 2. Defining a Command

```csharp
using RDH.Core.CQRS;

namespace DemoApplication.Handlers.AddUser;

public record AddUserCommand(
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth
) : ICommand;
```

### 3. Validating and Throwing Conflicts

Use `RDH.Core.Validation` extensions with FluentValidation to express business validation failures:

```csharp
using FluentValidation;
using RDH.Core.Validation;

// Throwing a conflict (HTTP 409 status code attached to ValidationException)
if (await userRepo.AnyAsync(e => e.Email == request.Email, cancellationToken))
{
    ValidationException.ThrowConflict("Email already exists");
}
```

### 4. Strongly Typed Identifier

```csharp
using RDH.Core;

namespace DemoDomain.Models;

public record UserId(Guid Value) : EntityId(Value);
```

### 5. Dispatching via `ISender`

```csharp
public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users", async ([FromBody] NewUser newUser, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(new AddUserCommand(
                newUser.FirstName,
                newUser.LastName,
                newUser.Email,
                DateOnly.FromDateTime(newUser.DateOfBirth.Value)), cancellationToken));
    }
}
```
