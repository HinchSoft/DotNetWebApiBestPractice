# RDH.Core

`RDH.Core` is the foundation package for the solution. It contains the reusable cross-cutting abstractions that support CQRS-style request handling, typed identifiers, validation, result models, and PII-aware logging behavior.

The package is organized into the following namespaces:

- `RDH.Core`
- `RDH.Core.CQRS`
- `RDH.Core.PIILogging`
- `RDH.Core.Results`
- `RDH.Core.Validation`

## Core

The `RDH.Core` namespace contains the fundamental types that other packages build on.

### EntityId
`EntityId` is an abstract record used to model a strongly typed identifier for an entity.

- It wraps a `Guid` value
- It provides a consistent `ToString()` representation
- It helps avoid accidental mixing of different entity IDs by forcing domain-specific types to derive from it

This makes entity references more explicit and safer than using raw GUID values throughout the system.

### ServiceExtensions
`ServiceExtensions` exposes the `AddCQRS` extension method for dependency injection.

This method scans one or more assemblies, finds concrete classes that implement the CQRS handler contracts, and registers them in the container. If no assembly is supplied, it defaults to the calling assembly.

The intent is to keep handler discovery automatic and reduce startup boilerplate in application code.

## CQRS

The `RDH.Core.CQRS` namespace contains the request/command/query model and the dispatch pipeline used by the solution.

### Request and handler contracts
The package includes the following core abstractions:

- `IRequest<TResponse>`
- `ICommand`
- `ICommand<TResponse>`
- `IQuery<TResponse>`
- `IAsyncEnumerableQuery<TResponse>`
- `IRequestHandler<TRequest, TResponse>`
- `ICommandHandler<TRequest>`
- `ICommandHandler<TRequest, TResponse>`
- `IQueryHandler<TRequest, TResponse>`
- `IAsyncEnumerableQueryHandler<TRequest, TResponse>`

These interfaces define the contracts for commands, queries, and their handlers without tying the application to a specific transport or framework.

### Sender
`ISender` and its implementation `Sender` are the execution entry points. They locate the correct handler in the dependency injection container and invoke it.

The sender also checks for a matching `IValidator<TRequest>` and runs it before executing the handler. If validation fails, a `ValidationException` is thrown and the handler is not called.

This creates a consistent pattern for validation and request execution across the application.

## PII Logging

The `RDH.Core.PIILogging` namespace provides data classification definitions used by the redaction and logging pipeline.

### PIIClassifications
`PIIClassifications` exposes the standard classification values:

- `Private`
- `Public`
- `Personal`

These are created using `Microsoft.Extensions.Compliance.Classification.DataClassification` and help downstream logging/redaction systems decide whether values should be masked or retained.

### Data classification attributes
The package also includes attributes to label object members:

- `PrivatePIIAttribute`
- `PersonalPIIAttribute`

These are useful when a model needs to explicitly declare that a property contains sensitive information.

## Results

The `RDH.Core.Results` namespace provides a lightweight result model for representing success and failure without forcing every validation or business case into an exception.

### Result
`Result` is a record that carries:

- `Errors`: a list of error messages
- `NotFound`: whether the operation represents a missing resource
- `Conflict`: whether the operation is in a conflict state
- `IsSuccess`: computed from whether any errors are present
- `IsFail`: the opposite of `IsSuccess`

`Result<TResult>` adds a typed `Value` payload to the same pattern.

This allows handlers to return structured outcomes that callers can inspect and react to in a consistent way.

### ResultBuilder
`ResultBuilder` and `ResultBuilder<TOut>` provide fluent accumulation of results.

They support:

- adding one or more errors
- marking a result as `NotFound`
- marking a result as `Conflict`
- merging nested result values
- constructing either a `Result` or typed `Result<T>`

This is especially helpful when a workflow needs to collect multiple validation or processing failures before returning a response.

## Validation

The `RDH.Core.Validation` namespace integrates FluentValidation with the application's shared validation patterns.

### ValidationExtensions
`ValidationExtensions` adds helper methods to `ValidationException` so validation errors can carry additional metadata about the API response.

Included functionality:

- `GetStatus()`: returns the status stored in the exception data, defaulting to `400`
- `GetStatusCode()`: returns a status text such as `Bad Request`
- `ThrowConflict(string message)`: creates a validation exception with status `409` and code `Conflict`

This makes it easier for API layers to convert validation failures into accurate HTTP responses.

### Options integration
The package also includes option-validation support:

- `OptionsBuilderExtensions.ValidateFluentValidation<TOptions>()`
- `OptionsValidatorBridge<TOptions>`

These types allow configuration values to be validated through a registered `IValidator<TOptions>` instance during startup. This keeps options validation aligned with the rest of the FluentValidation-based system.

## Typical usage

A typical pattern looks like this:

```csharp
services.AddCQRS(typeof(MyHandler).Assembly);

services.AddOptions<MySettings>()
    .BindConfiguration("MySettings")
    .ValidateFluentValidation();
```

The central idea of `RDH.Core` is to provide the reusable, low-level building blocks for the rest of the solution while keeping application code clean, testable, and consistent.
