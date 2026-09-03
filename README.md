# .Net WebApi Best Practice

This Repo is split into two parts; the demo application and the shared packages that are intended for reuse with real
world applications.

## Architecture

Vertical slice architecture is recommended as it helps keep related code together, but it still uses an onion skin
approach as the different layers are still relevant and should respect dependency rules.

### Domain or Core

This is the base layer it should have no dependencies on any other layer. It contains things like models and model
validation rules, for example a `User` model with `Firstname` and `Lastname` properties and Validation rules the check
both names are provided and of a minimum length.

### Application Layer

This is where Handlers, services and Dtos that are specific to the application are implemented, Interfaces for services
provided by other layers e.g. infrastructure are defined here, these would be things like databases, third party APIs
caching ect.

This is the layer where the majority of the vertical slice architecture will come in, Handlers having command and
response objects grouped together for example. This layer can reference the Domain layer

### Infrastructure

This is where database access and third party APIs are implemented basically anything infrastructure related. This layer
can reference the Domain and Application layers

### Presentation

This layer is where the application is surfaced, its the web pages or API interface or both, depending on the
application its the point of access for users. This layer can reference all other layers as its normally the one that
pairs a definition with an interface (usually bu configuring using dependency injection)

## Architecture Tests

In the Tests directory there is an example `ArchitectureTests` project. This demonstrates how to use the unit tests to
validate and enforce the desired architecture.

## Logging

When implementing logging consideration should be given to redacting personal identifiable information (PII). This can
be done by registering redactors and PII classifications, then marking up the items of data that need redacting with a
classification. The native .net logging supports this with very little configuration, especially if log templates are
used e.g.

``` C#
public static partial class UserLogging
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "User {UserId} created with email {Email}.")]
    public static partial void UserCreated(
        this ILogger logger,
        UserId userId,
        [PersonalPII] string email);
}
```
