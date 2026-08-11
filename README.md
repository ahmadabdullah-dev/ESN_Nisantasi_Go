# Social Plan Platform

- A full-stack Social Plan Management Platform where users can add their own plans and other users can join them, built with a 3-layer .NET Web API backend and a React web frontend.
- The application enforces a strict role hierarchy using ASP.NET Core Identity and features a modern, responsive UI built with Material UI and TanStack React Query.

## Authentication & Role Permissions

### User Authentication

- Supports secure Login and Logout for all users.
- Uses ASP.NET Core Identity for authentication and session management.

### Role Hierarchy

**Super Admin**
- Can register new Admin users.
- Can register new Member users.
- Has full administrative access across the platform.

**Admin**
- Can register new Member users.
- Cannot register Admin accounts.

**Member**
- Can browse, create, join (RSVP), and leave plans.
- Can view personal profile and plans history.

---

## Backend Architecture (.NET Web API)

Built using a 3-Layer Architecture (API, Business, DataAccess) with clean code principles.

### 1. Layers & Responsibilities

**API Layer**
- Handles HTTP requests, CORS policies, rate limiting, and endpoint routing.
- Contains `BaseApiController` for standardizing controller responses.

**Business Layer**
- Contains application logic, services, interfaces, DTOs and the `Result<T>` pattern.

**DataAccess Layer**
- Contains `AppDbContext`, repositories, interfaces, migrations, configurations, and data seeding.

### 2. Entities & Data Model

| Entity | Description |
|---|---|
| `AppUser` | Extends `IdentityUser` with `FirstName`, `LastName`, `Country`, `Department`, `IsActive`, and `CreatedAt`. |
| `Plan` | Represents a social event with title, description, location, date/time. |
| `PlanParticipant` | Union entity manages the Many-to-Many relationship between `AppUser` and `Plan`. |

### 3. Key Backend Implementation Highlights

- **Database-Level Filtering & Pagination** — Data is filtered directly at the SQL database level before being retrieved into memory, using `PagedList` and `Select`.
- **Interfaces** — Are used for all services (`IAdminService`, `IAuthService`, `IPlanService`, `IUserService`, `IEmailService`) and repository contracts.
- **Result Pattern** — Standardized `Result<T>` wrapper for returning success/error responses cleanly without throwing unhandled exceptions.
- **Data Transfer Objects (DTOs)** — Strict separation between database entities and API request/response payloads.
- **Data Seeder (`DataSeeder.cs`)** — Automatically seeds initial roles (`SuperAdmin`, `Admin`, `Member`) and default test users on application startup.
- **Google SMTP Email Service** — Integrated asynchronous email delivery for sending emails to users.
- **Configuration Management (`IOptions`)** — Uses the `IOptions<T>` pattern to read configuration settings from environment variables or `appsettings.json`.
- **Clean Code Structure** — Each layer features its own `DependencyInjection.cs` extension file and `GlobalUsings.cs` to eliminate boilerplate.
- **Security Controls**
  - Rate Limiting: Applied to authentication endpoints (`auth` policy) to protect against brute-force attacks.
  - CORS Policy: Configured `AllowWeb` policy to restrict origin access securely.

---

## Frontend Architecture

Built with React, TypeScript, TanStack React Query, and Material UI.

### Directory Layout

The frontend codebase is organized into `app`, `features`, and `lib` directories:

```
WEB/src/
├── app/          # Navigation, Header, Footer, and Layout components
├── features/     # Feature-based pages and UI components
└── lib/          # Custom hooks, API agent, TypeScript types, and MUI theme
```

---

## Seeded Test Credentials

The database automatically seeds test accounts upon startup:

| Role | Email | Password | Account Capabilities |
|---|---|---|---|
| SuperAdmin | `superadmin1@test.com` | `Pa$$w0rd` | Can register Admin and Member accounts |
| Admin | `admin1@test.com` | `Pa$$w0rd` | Can register Member accounts |
| Member | `member1@test.com` | `Pa$$w0rd` | Can create plans, RSVP, and view profiles |

---

## How to Run

### Run Frontend React Web

```powershell
cd WEB
npm install
npm run dev
```

Web Application runs at `http://localhost:3000`

Set the configurations in `appsettings.json`.

```json
"ConnectionStrings": {
  "DefaultConnection": ""
},
"EmailConfiguration": {
  "From": "",
  "smtpServer": "",
  "Port": "",
  "UserName": "",
  "Password": "",
  "EnableSsl": ""
},
```

## Run Database Migrations

Run these commands from the **solution root**.

**Add a migration:**

```powershell
dotnet ef migrations add Mig_1 `
  --project .\DataAccess\DataAccess.csproj `
  --startup-project .\API\API.csproj
```

**Apply migrations:**

```powershell
dotnet ef database update `
  --project .\DataAccess\DataAccess.csproj `
  --startup-project .\API\API.csproj
```
