# ⚡ PortfolioEngine (.NET 10 Clean Architecture)

> A modern, headless Portfolio Content Management Engine and API built with **.NET 10**, **EF Core**, **Clean Architecture**, **ASP.NET Core Identity**, and interactive **Scalar API Documentation**.

---

## 🏗 Architecture Overview

The system is designed following strict **Clean Architecture** principles to separate concerns into predictable, decoupled layers:




              ┌─────────────────────────────────┐
              │      PortfolioEngine.Web        │ (MVC Admin & REST API)
              └────────────────┬────────────────┘
                               │
              ┌────────────────▼────────────────┐
              │   PortfolioEngine.Application   │ (Interfaces, DTOs, Services)
              └────────────────┬────────────────┘
                               │
              ┌────────────────▼────────────────┐
              │     PortfolioEngine.Domain      │ (Entities & Auditing Core)
              └────────────────▲────────────────┘
                               │
              ┌────────────────┴────────────────┐
              │ PortfolioEngine.Infrastructure  │ (EF Core, Repositories, DbContext)
              └─────────────────────────────────┘


---

## 🛠 Key Features

- **Clean Architecture Solution Structure:** 4-layer isolation (Domain, Application, Infrastructure, Web).
- **Headless REST API (`/api/v1`):** Endpoints to serve projects, tech stacks, and experiences to modern frontend frameworks (React, Next.js, Vue).
- **Interactive OpenAPI Documentation:** Integrated **Scalar UI** theme (`/scalar/v1`) for real-time interactive API testing.
- **Generic Repository & Unit of Work:** Managed database transaction boundaries and reusable query abstractions.
- **EF Core Interceptors for Soft Delete & Auditing:** Automated population of `CreatedAtUtc`, `UpdatedAtUtc`, and global query filters for soft deletion.
- **ASP.NET Core Identity Authentication:** Cookie-based session authentication for MVC Admin views with customized `ApplicationUser` claims.
- **Containerized Development Ready:** Full support for Podman & Docker via Compose files.

---

## 🚀 Quick Start (Local Development)

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [MS SQL Server] or [Podman / Docker]

### 1. Clone & Restore
```bash
git clone [https://github.com/your-username/PortfolioEngine.git](https://github.com/your-username/PortfolioEngine.git)
cd PortfolioEngine
dotnet restore


2. Run Database via Container

podman-compose up -d database
# Or using Docker:
# docker compose up -d database

3. Apply Migrations & Run Application

dotnet ef database update --project src/PortfolioEngine.Infrastructure --startup-project src/PortfolioEngine.Web
dotnet run --project src/PortfolioEngine.Web



📑 API Reference & Endpoints

    Verb    Endpoint                                Description
    GET     /api/v1/projects                        Fetch all non-deleted portfolio projects
    GET     /api/v1/projects/featured               Fetch only featured portfolio projects
    GET     /api/v1/projects/slug/{slug}            Fetch single project by URL slug
    GET     /api/v1/techstacks                      Fetch skills and proficiency percentages
    GET     /admin/projects                         Access MVC Dashboard (Requires Authentication)
    GET     /scalar/v1                              Open Interactive Scalar API Reference



👤 Admin Login (Seeded Defaults)

# Email: admin@portfolioengine.local

# Password: Admin@123456#


developed by Eslam Aldura
github : [https://github.com/EslamAl-dura]