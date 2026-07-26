# Project Management System API

An ASP.NET Core Web API for managing projects and tasks, built with Clean Architecture.

## Features	

- JWT authentication — register an account, log in, get a token, use it everywhere
- Full CRUD for projects and tasks, scoped to the authenticated user
- FluentValidation on all inputs (create, update, auth)
- Global exception handling — `NotFoundException` maps to 404, `BadRequestException` to 400
- EF Core with SQL Server for persistence
- Swagger UI for exploring the API

## Tech

- .NET 9 / ASP.NET Core Web API
- SQL Server + EF Core 9
- JWT Bearer auth (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- FluentValidation
- BCrypt.Net for password hashing

## Project structure

```
job_test/
├── Controllers/          # HTTP layer — thin, just auth + routing
├── Application/
│   ├── DTOs/             # What the API accepts and returns
│   ├── Interfaces/       # Service contracts
│   ├── Services/         # Business logic
│   ├── Validators/       # FluentValidation rules
│   └── Exceptions/       # NotFoundException, BadRequestException
├── Domain/
│   ├── Models/           # User, Project, TaskItem
│   └── Enums/            # ProjectTaskStatus, TaskPriority
└── Infrastructure/
    ├── Authentication/   # JwtService
    └── Persistence/      # ApplicationDbContext, migrations
```

The dependency direction goes inward: Controllers → Application → Domain. Infrastructure is referenced by Application services (via DbContext) and wired up in Program.cs.

## Getting started

**1. Set your connection string** in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=JobTestDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

**2. Set your JWT config** in `appsettings.json`:

```json
"Jwt": {
  "Key": "your-secret-key-at-least-32-chars",
  "Issuer": "job-test-api",
  "Audience": "job-test-client"
}
```

**3. Apply migrations:**

```powershell
Add-Migration Init
Update-Database
```

**4. Run the project** — Swagger opens at `https://localhost:{port}/swagger`.

## Using the API

1. `POST /api/auth/register` — create an account
2. `POST /api/auth/login` — get a JWT token
3. Click "Authorize" in Swagger and paste the token
4. Create projects, then tasks inside them

All project and task endpoints are scoped to the logged-in user — you can only see and modify your own data.

## Assumptions & Design Decisions

- **JWT authentication & per-user ownership** — The spec only required a task management API, but JWT auth and data isolation by user were added to make it a realistic multi-user scenario. Every project and task query filters by the authenticated user's ID to ensure data privacy.
- **Enums stored as strings in the database** — `ProjectTaskStatus` and `TaskPriority` use `HasConversion<string>()` in EF Core so the stored values are human-readable (e.g. `"InProgress"`, `"High"`) rather than opaque integers. This makes the database easier to inspect and debug.
- **Service layer over DbContext** — Instead of a full Repository + Unit of Work pattern, the service layer calls `DbContext` directly. For a project of this scope the abstraction would add ceremony without meaningful benefit.
- **FluentValidation for input validation** — All DTOs are validated via FluentValidation before reaching the service layer. This keeps controllers thin and validation logic reusable.
- **Global exception middleware** — Custom `NotFoundException` and `BadRequestException` are caught by middleware and mapped to appropriate HTTP status codes (404, 400), avoiding try/catch noise in controllers.

## What I'd improve with more time

- **Unit & integration tests** — The project has no test coverage. I'd add xUnit tests for services (unit tests with mocked DbContext) and integration tests for the API endpoints.
- **Pagination** — List endpoints (`GET /api/projects`, `GET /api/tasks/project/{id}`, `GET /api/tasks`) return all results at once. Adding `page` and `pageSize` query parameters with a paginated response wrapper would scale better.
- **Request logging & audit trail** — Middleware for logging request duration, and a `CreatedBy`/`UpdatedAt` timestamp on entities for basic auditing.
- **Rate limiting** — Protect the auth endpoints from brute-force attacks with a rate limiter (e.g. `AspNetCoreRateLimit` or built-in .NET 7+ rate limiting).
- **Split into microservices** — If the domain grew, the project and task bounded contexts could each become their own service communicating via a message broker, with a shared auth service.
- **Docker Compose** — A `docker-compose.yml` with the API + SQL Server container would eliminate the need to manually set up a local database.