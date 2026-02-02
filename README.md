# FemvedBackend

Production-grade .NET 10 Web API backend following Clean Architecture and SOLID principles. The solution separates `Domain`, `Application`, `Infrastructure`, and `API` layers and targets PostgreSQL with EF Core Fluent API.

## Tech Stack
- .NET 10
- ASP.NET Core Web API
- EF Core (Fluent API)
- PostgreSQL
- JWT authentication
- Serilog (PostgreSQL sink)

## Solution Structure
- `FemvedBackend.Domain` — Entities/enums only
- `FemvedBackend.Application` — Use cases, validators, contracts
- `FemvedBackend.Infrastructure` — EF Core, repositories, identity, services
- `FemvedBackend.Api` — Controllers and composition root

## Configuration
Update `src/FemvedBackend.Api/appsettings.json`:
- `ConnectionStrings:DefaultConnection`
- `ConnectionStrings:SerilogConnection`
- `Jwt` settings

Local overrides go in `appsettings.Development.json`.

## Database
Schema reference: `src/FemvedBackend.Infrastructure/Persistence/Docs/db-schema.txt`

## Logging
Serilog is configured to write to PostgreSQL. Request context (path, user ID) is enriched via `LogContext`.

## Validation & Errors
Application-level validation throws a custom `ValidationException` and returns a consistent ProblemDetails-style response with HTTP 400.

## Running
Use Visual Studio to launch `FemvedBackend.Api`. Ensure PostgreSQL is reachable and the schema exists before starting the API.

## Testing
Unit tests are located under:
- `src/Femved.Application.Tests`
- `src/FemvedBackend.Api.Tests`
