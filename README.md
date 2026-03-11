# UniMeet - Backend

> **Project is under active development. Some features may change.**

UniMeet is a platform for university students to connect, meet up and communicate based on shared interests and courses. Students register with their university e-mail address, are enrolled in their university's courses, and can find other students with overlapping enrolments or interests and message them in real time.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 (ASP.NET Core) |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Authentication | JWT Bearer tokens + Refresh Tokens |
---

## Architecture

The backend follows a **Modular Monolith** architecture. A lightweight `ModularSystem` engine discovers all `IModule` implementations at startup via reflection, reads each module's configuration section, and — if the module is enabled — calls `Start()` followed by `RegisterServices()`. This keeps every module isolated while sharing a single ASP.NET Core host.

```
src/
├── ModularSystem/          # Module engine (IModule, discovery, lifecycle)
├── UniMeet.Shared/         # Cross-cutting abstractions (mediator, exceptions)
├── UniMeet.API/            # ASP.NET Core host, controllers, SignalR hubs, middlewares
└── Modules/
    ├── UserModule/
    ├── PermissionsModule/
    ├── UniversityModule/
    ├── UserEnrollmentModule/
    ├── MailingModule/
    └── MessagingModule/
```

Each module is split into four projects following **Clean Architecture**:

| Layer | Suffix | Responsibility |
|---|---|---|
| Entry point | `<ModuleName>` | `IModule` registration, DI wiring |
| Application | `.Application` | Use-cases / handlers |
| Domain | `.Domain` | Entities, repository interfaces, domain logic |
| Infrastructure | `.Infrastructure` | EF Core contexts, repository implementations |

---

## Modules

### User Module
Handles the full user lifecycle: registration, e-mail confirmation, login, token refresh, logout, password reset, user details (bio, avatar, sex) and interests. JWT tokens are issued here; the module also exposes the auth scheme used by the rest of the application.

Key domain concepts: `User`, `UserDetail`, `ConfirmationCode`, `PasswordResetCode`, `RefreshToken`, `Interest`.

### Permissions Module
Role-based access control. Groups are collections of named permissions. Every permission maps to a specific operation exposed by another module (e.g. `MessagingModule.SendMessage`). Two default groups ship out of the box: **User** and **Admin**.

Key domain concepts: `Group`, `Permission`.

### University Module
Manages university data: universities, departments, fields of study, and allowed e-mail domains (used during registration to verify a student's address). Admins can create/update/delete all university-related data.

Key domain concepts: `University`, `Department`, `FieldOfStudy`, `AllowedEmailDomain`.

### User Enrollment Module
Tracks which university courses (fields of study) a user is enrolled in. Provides the basis for matching students who share the same courses.

Key domain concepts: `UserAffiliation`.

### Mailing Module
Sends transactional e-mails (confirmation codes, password-reset links) via SMTP. Stateless — no database required.

### Messaging Module
Real-time private messaging between users. Conversations are created on demand; messages are persisted in the database and delivered via SignalR. Supports marking messages as read.

Key domain concepts: `Conversation`, `Message`.

---

## Database 
![ER Diagram](imgs/database.png)

Every module that requires persistence uses its own **EF Core DbContext** connected to the shared PostgreSQL instance. Migrations are stored inside each module's `Infrastructure` project.

---

## Configuration

All configuration lives in `appsettings.json` (and environment-variable overrides when running via Docker). Each module has its own section under `Modules`:

```jsonc
{
  "Urls": "https://localhost:5001",
  "Modules": {
    "UserModule": {
      "Enabled": true,
      "DbConnectionString": "<connection-string>",
      "WebsiteUrl": "http://localhost",
      "Auth": {
        "Secret": "<jwt-secret>",
        "Issuer": "<issuer>",
        "Audience": "<audience>"
      }
    },
    "UniversityModule": {
      "Enabled": true,
      "DbConnectionString": "<connection-string>"
    },
    "UserEnrollmentModule": {
      "Enabled": true,
      "DbConnectionString": "<connection-string>"
    },
    "PermissionsModule": {
      "Enabled": true,
      "DbConnectionString": "<connection-string>",
      "SeedDatabase": false,
      "DefaultGroups": { ... }
    },
    "MailingModule": {
      "Enabled": true,
      "Smtp": {
        "Host": "<smtp-host>",
        "Port": 587,
        "Username": "<username>",
        "Password": "<password>",
        "SenderName": "UniMeet"
      }
    },
    "MessagingModule": {
      "Enabled": true,
      "DbConnectionString": "<connection-string>"
    }
  }
}
```

### Running with Docker Compose

Copy `.env.example` to `.env` and fill in the required values:

| Variable | Description |
|---|---|
| `POSTGRES_USER` | PostgreSQL username |
| `POSTGRES_PASSWORD` | PostgreSQL password |
| `POSTGRES_DB` | Database name |
| `USER_MODULE_AUTH_SECRET` | JWT signing secret |
| `USER_MODULE_WEBSITE_URL` | Base URL used in confirmation e-mails |
| `MAILING_MODULE_SMTP_HOST` | SMTP server host |
| `MAILING_MODULE_SMTP_PORT` | SMTP server port |
| `MAILING_MODULE_SMTP_USERNAME` | SMTP username |
| `MAILING_MODULE_SMTP_PASSWORD` | SMTP password |
| `MAILING_MODULE_SMTP_SENDER_NAME` | Display name for outgoing e-mails |

Then:

```bash
docker compose up --build
```

The API is available on ports `8080` (HTTP) and `8081` (HTTPS).

---

## API Endpoints
See: [API Documentation](API_Documentation.pdf)

Swagger UI is also available at `/swagger` when running in Development mode.