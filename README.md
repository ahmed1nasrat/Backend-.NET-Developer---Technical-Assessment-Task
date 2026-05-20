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