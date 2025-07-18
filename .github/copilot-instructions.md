# Copilot Instructions for Netflex

## Project Architecture

- **Solution Structure:** The codebase is organized as a multi-project .NET solution (`src/Netflex.sln`) with clear separation into Core, Infrastructure, Persistence, Presentation, and Shared layers.
- **Core Layer:** Contains application logic, DTOs, interfaces, and use cases. Key directories: `Netflex.Application/DTOs`, `Netflex.Application/Interfaces`, `Netflex.Application/UseCases`.
- **Domain Layer:** Defines entities, value objects, enumerations, and domain events (`Netflex.Domain`).
- **Infrastructure Layer:** Implements services and settings, including external integrations (`Netflex.Infrastructure/Services`, `Netflex.Infrastructure/Settings`).
- **Persistence Layer:** Handles data access, migrations, and repository implementations (`Netflex.Persistence`).
- **Presentation Layer:** ASP.NET Web API project (`Netflex.WebAPI`) with endpoints, middleware, and configuration files.
- **Shared Layer:** Common utilities, exceptions, and CQRS behaviors (`Netflex.Shared`).

## Developer Workflows

- **Build:** Use the VS Code task labeled `build` or run `dotnet build src/Netflex.sln`.
- **Run/Debug:** Start the Web API from `Netflex.WebAPI/Program.cs`. Use Dockerfiles for containerization if needed.
- **Configuration:** App settings are managed in `Netflex.WebAPI/appsettings.json` and `appsettings.Development.json`.
- **Dependency Injection:** All major components register dependencies via `DependencyInjection.cs` in each project.

## Project-Specific Conventions

- **DTOs:** All data transfer objects are in `Netflex.Application/DTOs`.
- **Interfaces:** Service and repository interfaces are in `Netflex.Application/Interfaces`.
- **Exception Handling:** Custom exceptions are organized under `Common/Exceptions` and other relevant folders.
- **CQRS:** Shared CQRS behaviors are in `Netflex.Shared/CQRS`.
- **Global Usings:** Each project uses a `GlobalUsing.cs` for common imports.

## Integration Points

- **External Services:** Integrations (e.g., cloud storage, email, social) are abstracted via interfaces in `Netflex.Application/Interfaces` and implemented in `Netflex.Infrastructure/Services`.
- **Database:** Entity configurations and migrations are in `Netflex.Persistence/Configurations` and `Netflex.Persistence/Migrations`.
- **Docker:** Use `compose.yaml` and `Dockerfile` for local development and deployment.

## Patterns & Examples

- **Dependency Injection:**
  - Register services in `DependencyInjection.cs` (e.g., `Netflex.Infrastructure/DependencyInjection.cs`).
- **DTO Usage:**
  - Example: `ActorDto`, `MovieDto` in `Netflex.Application/DTOs`.
- **Custom Middleware:**
  - Add middleware in `Netflex.WebAPI/Middleware`.
- **Endpoints:**
  - Define API endpoints in `Netflex.WebAPI/Endpoints`.

## Key Files & Directories

- `src/Netflex.sln` — Solution file
- `Netflex.Application/DTOs/` — Data transfer objects
- `Netflex.Application/Interfaces/` — Service/repository interfaces
- `Netflex.Infrastructure/Services/` — Service implementations
- `Netflex.Persistence/` — Data access and migrations
- `Netflex.WebAPI/` — ASP.NET API project
- `Netflex.Shared/` — Shared utilities and CQRS

---

_Review and update these instructions as the architecture evolves. For unclear or missing conventions, ask maintainers for clarification._
