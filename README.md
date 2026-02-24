# .Net WebApi Best Practice

A simple CQRS sender, with standard service state capturing and 
automatic discovery of CQRS handlers and api registration. And 
a minimal, example Web API demonstrating best practices for building small-to-medium .NET 10 APIs using:
- A lightweight CQRS-style sender (`CQRSServices`)
- Structured API result types and HTTP response mapping (`CQRSServices\ApiResults`)
- Self-documented OpenAPI + Scalar API reference for client generation
- Minimal APIs 

This repository is intentionally small and focused to illustrate patterns you can adopt in production services.

## Contents (high level)
- `DemoApi/` — The runnable API project (entry point: `Program.cs`, demo endpoints in `Endpoints/`, handler examples in `Handlers/`, lightweight `Services/`).
- `CQRSServices/` — Reusable libraries:
  - `CQRS\ISender.cs`, `CQRS\Sender.cs` — small send/dispatch abstraction.
  - `Results\Result.cs`, `ResultBuilder.cs` — result shape and helpers.
  - `ApiResults\*` — mapping results to HTTP responses.
  - `ServiceExtensions.cs` — registration helpers for DI.
- `CQRSServicesTests/` — unit tests covering result mapping and builder behaviour.

Projects target `.NET 10`.


