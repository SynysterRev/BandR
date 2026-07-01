# BandR 🎸

**BandR** is a REST API for connecting musicians. Post announcements, find bandmates, and message other musicians, all in one place.

> ⚠️ This project is actively under development. Frontend coming soon.

---

## Features

- **Authentication**: JWT-based auth with access & refresh tokens
- **Musician profiles**: username, bio, location, instruments, styles, tags
- **Announcements**: post and browse ads with filtering and pagination
- **Messaging**: start conversations directly from an announcement *(in progress)*
- **Reference data**: curated list of instruments, music styles and tags

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10 |
| Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL 16 |
| Auth | ASP.NET Identity + JWT Bearer |
| Validation | FluentValidation |
| API Docs | Scalar / OpenAPI |
| Containerization | Docker + Docker Compose |
| CI/CD | GitHub Actions |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run locally

**1. Clone the repo**
```bash
git clone https://github.com/SynysterRev/BandR.git
cd BandR
```

**2. Start the database**
```bash
docker compose up -d
```

**3. Apply migrations**
```bash
dotnet ef database update
```

**4. Run the API**
```bash
dotnet run --project BandR
```

**5. Open the API docs**

Navigate to `https://localhost:7294/scalar/v1`

---

## API Overview

### Auth
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/account/register` | Register | ❌ |
| POST | `/api/account/login` | Login | ❌ |
| POST | `/api/account/refresh` | Refresh access token | ❌ |
| POST | `/api/account/logout` | Revoke refresh token | ✅ |

### Musicians
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/musicians` | List musicians | ✅ |
| GET | `/api/musicians/{id}` | Get musician by ID | ✅ |
| POST | `/api/musicians` | Create musician profile | ✅ |
| DELETE | `/api/musicians/{id}` | Delete musician profile | ✅ |
| GET | `/api/musicians/me/announcements` | Get my announcements | ✅ |
| GET | `/api/musicians/{id}/announcements` | Get announcements by musician | ✅ |

### Announcements
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/announcements` | List announcements (paginated) | ✅ |
| GET | `/api/announcements/{id}` | Get announcement by ID | ✅ |
| POST | `/api/announcements` | Create announcement | ✅ |
| DELETE | `/api/announcements/{id}` | Delete announcement | ✅ |

---

## Database Schema

**Core entities:** `ApplicationUser` · `Musician` · `Announcement` · `Location`

**Reference data:** `Instrument` · `Style` · `Tag`

**Junction tables:** `musician_instruments` · `musician_styles` · `musician_tags` · `announcement_instruments` · `announcement_styles` · `announcement_tags`

**Auth:** `RefreshToken` + ASP.NET Identity tables

---

## Roadmap

### MVP
- [x] Authentication (JWT + refresh tokens)
- [x] Musician profiles
- [x] Announcements with pagination
- [ ] Search & filtering
- [ ] Messaging
- [ ] GitHub Actions CI
- [ ] Azure deployment

---

## Running Tests

```bash
dotnet test
```

> Integration tests use [Testcontainers](https://testcontainers.com/) and require Docker to be running.

---

## Environment Variables

Configure in `appsettings.Development.json` :

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=bandr;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-min-32-chars",
    "Issuer": "https://localhost:7294",
    "Audience": "https://localhost:7294",
    "ExpirationInMinutes": 60,
    "RefreshExpiryDays": 7
  }
}
```

---

*Built with .NET 10 · PostgreSQL · Docker*
