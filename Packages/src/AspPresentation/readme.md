# RDH.AspPresentation

`RDH.AspPresentation` is an ASP.NET Core presentation layer library. It provides dynamic endpoint discovery using `IEndpoint`, global RFC 7807 problem details handling, PII redaction loggers, and HTTP API utilities.

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package RDH.AspPresentation
```

Or via the Package Manager Console:

```powershell
Install-Package RDH.AspPresentation
```

---

## Key Features

- **Modular Endpoint Discovery (`IEndpoint`):** Define endpoints across feature folders without monolithic controller classes or giant `Program.cs` files. `builder.AddEndpoints()` automatically scans and registers them.
- **Dynamic Mapping (`app.MapEndpoints()`):** Maps all registered endpoints at startup, with optional support for `RouteGroupBuilder` (e.g. for versioning or authorization groups).
- **RFC 7807 Problem Details:** `builder.AddGlobalProblemDetails()` configures standardized error formats with correlation IDs and handles `ValidationException` as well as unhandled exceptions.
- **PII Redaction Logging:** `services.AddRedactionLogging()` sets up full-star and partial-star redaction for sensitive fields tagged with `[PrivatePII]` and `[PersonalPII]`.
- **API Response Utilities:** Standardized headers, pagination models, and result transformations.

---

## Sample Code

### 1. Defining a Feature Endpoint with `IEndpoint`

Implement `IEndpoint` inside your feature slice to configure route handlers:

```csharp
using DemoApi.Endpoints.Dtos;
using DemoApplication.Handlers.AddUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RDH.AspPresentation.Endpoints;
using RDH.Core.CQRS;

namespace DemoApi.Endpoints;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users",
            async ([FromBody] NewUser newUser, ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new AddUserCommand(
                    newUser.FirstName,
                    newUser.LastName,
                    newUser.Email,
                    DateOnly.FromDateTime(newUser.DateOfBirth!.Value)), cancellationToken));
    }
}
```

### 2. Startup Configuration in `Program.cs`

Wire up presentation services with the provided extension methods:

```csharp
var builder = WebApplication.CreateBuilder(args);

// PII redaction logging
builder.Services.AddRedactionLogging();

// Standardized RFC 7807 Problem Details and exception handlers
builder.AddGlobalProblemDetails();

// Discover and register all IEndpoint implementations
builder.AddEndpoints();

// API utilities for response envelopes and pagination
builder.AddApiResponses();

var app = builder.Build();

// Enable exception and status code middleware
app.UseExceptionHandler();
app.UseStatusCodePages();

// Map all discovered endpoints
app.MapEndpoints();

app.Run();
```
