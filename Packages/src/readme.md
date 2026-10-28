# Packages/src Architecture

This folder contains a small package library set that implements a layered .NET application architecture for an ASP.NET Core API. The projects are organized around separation of concerns rather than a single monolithic application: foundational abstractions live in the core package, application contracts and workflow logic live in the application layer, data access and web integration are implemented in infrastructure-specific packages.

## High-level dependency flow

The overall direction of dependencies is intentionally inward:

- `Core` provides shared abstractions and infrastructure primitives.
- `ApplicationLayer` depends on `Core` and defines the application contracts and repository abstractions that business logic depends on.
- `EFInfrastructure` implements the repository and unit-of-work abstractions using Entity Framework Core.
- `AspPresentation` exposes HTTP endpoints and ASP.NET Core integration for the application layer.
- `AspireDefaults` provides startup/migration support for hosted services.
- `TestHelper` supports validation and architecture tests.

```text
AspPresentation --> ApplicationLayer --> Core
         \--> EFInfrastructure --> ApplicationLayer
```

## Project responsibilities

### Core
`RDH.Core` is the foundational package. It contains the common building blocks used across the solution:

- `CQRS` abstractions (`ISender`, request/command/query handlers), allowing the application to dispatch behavior through a common mediator-style interface.
- validation support via FluentValidation patterns and exception handling.
- result models and helper types used to standardize responses.
- PII-oriented logging/redaction classes, centered on classifications such as public, personal, and private data.
- service-registration helpers such as `AddCQRS`.

This project is intentionally framework-agnostic and should not depend on ASP.NET Core or EF Core.

### ApplicationLayer
`RDH.ApplicationLayer` contains the application-facing contracts and repository boundaries used by business logic and API handlers:

- repository abstractions (`IReadRepository<TEntity>`, `IWriteRepository<TEntity>`, `IUnitOfWork`)
- pagination metadata and response helper contracts (`IApiUtilities`, `Pagination`)
- use-case and service-oriented abstractions that are independent from the persistence technology

This package acts as the boundary between domain/application logic and its infrastructure implementation. It references `Core` but does not directly reference EF Core or ASP.NET Core.

### EFInfrastructure
`RDH.EFInfrastructure` implements the data access layer using Entity Framework Core:

- `RepositoryBase<TEntity, TContext>` provides generic CRUD/query behavior against a `DbContext`
- `UnitOfWorkBase<TContext>` centralizes persistence through a single save point
- repositories are implemented against the interfaces declared in `ApplicationLayer.Data`

This is where the application-agnostic repository contracts become concrete database implementations.

### AspPresentation
`RDH.AspPresentation` is the HTTP-facing layer. It contains the ASP.NET Core integration pieces that let endpoints be discovered and mapped dynamically:

- endpoint registration via `AddEndpoints` and `MapEndpoints`
- dynamic endpoint discovery through `IEndpoint` implementations
- problem-details configuration and global exception handling
- HTTP response utilities such as API utilities and pagination headers
- logging redaction setup for PII-sensitive information

In practice, the web layer is thin: it adapts application services and endpoints to ASP.NET Core conventions without owning the business logic itself.

### AspireDefaults
`AspireDefaults` provides startup support for database migration and initialization scenarios in a hosted environment.

- `MigrationWorker<TContext>` runs migrations and optional seed logic in a background service
- execution is tied to the host lifetime so the app can stop cleanly after initialization
- this package is useful when the solution needs standard database bootstrapping behavior in a distributed or hosted environment

### TestHelper
`TestHelper` provides test support utilities focused on validation and architecture enforcement. It includes:

- architecture assertions
- OS-specific test support helpers
- libraries such as xUnit and FluentAssertions to support consistent verification of service contracts and boundaries

## Architectural pattern in use

The overall design is a modern layered, service-oriented architecture with a CQRS-style command/query dispatch layer and a repository/unit-of-work data access abstraction. The key design intent is:

1. application behavior is expressed through abstractions rather than concrete infrastructure
2. infrastructure-specific concerns (EF Core, ASP.NET Core hosting, migration startup) are isolated behind interfaces or extension methods
3. the web layer remains thin and acts as a connector to the application layer

This makes the packages reusable, testable, and easier to adapt to different runtime environments while keeping the implementation patterns consistent across the solution.
