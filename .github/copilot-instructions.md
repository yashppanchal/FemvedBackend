# Copilot Instructions

## General Guidelines
- Use clear and concise comments to describe the purpose of functions and classes.
- Follow consistent naming conventions for variables, methods, and classes.
- Do not invent tables/columns/JSON fields; use only provided schemas/contracts.
- Do not modify authentication/users/roles/existing APIs; follow instructions strictly without asking questions.

## Code Style
- Adhere to the .NET coding standards for formatting and structure.
- Use specific formatting rules, such as indentation and spacing, to enhance readability.

## Project-Specific Rules
- Build a production-grade .NET 10 Web API backend using Clean Architecture and SOLID principles.
- Follow Clean Architecture rules: Domain has only entities/enums, Application only interfaces/contracts, Infrastructure implements Application with EF Core (Fluent API, PostgreSQL), API depends only on Application interfaces.
- Utilize PostgreSQL with EF Core Fluent API for database interactions.
- Implement JWT authentication for secure access, using PBKDF2 via ASP.NET Core cryptography APIs for password hashing.
- Refresh tokens must be persisted, hashed, rotated, and revocable.
- Apply role/policy-based authorization to manage user permissions.
- Design the application as a modular monolith with the intention to scale into microservices in the future.
- Ensure that payments belong to orders and access is managed via user_product_access.
- Implement a strategy for payment gateways and use domain events for notifications.
- Keep controllers thin and avoid placing business logic within them.
- Use async/await in the API layer, and avoid using DbContext or repositories in controllers.
- Prefer separate application handlers per use case over a single AuthService.
- Implement email sending via IEmailSender with a console/logging stub.
- Add ICurrentUserService abstraction now.
- Set the base route for the API to `/api/guided`.