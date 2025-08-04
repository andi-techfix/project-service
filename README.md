# ProjectService

An ASP.NET Core Web API for managing projects and indicators, backed by MongoDB.

---

## Features

* Query projects by user IDs
* Aggregate and return top indicators
* ASP.NET Core + MongoDB
* MediatR-based CQRS query handlers
* Resilient HTTP client for UserService calls (Polly)
* Exception-handling middleware
* Swagger UI for API exploration
* Dockerfile for containerized deployment

---

## Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [MongoDB 5](https://www.mongodb.com/try/download/community) (or Docker)
* (Optional) [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)

---

## Getting Started

### 1. Clone

```bash
git clone https://github.com/andi-techfix/project-service.git
cd project-service/ProjectService
```

### 2. Configure MongoDB

Copy `appsettings.Development.json` and update the `MongoDb` section:

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "projectsdb"
  }
}
```

### 3. Configure UserService URL

In `appsettings.Development.json`, set:

```json
{
  "UserServiceUrl": "http://localhost:5000"
}
```

### 4. Run Locally

```bash
dotnet run
```

The API will be listening on `https://localhost:5101` and `http://localhost:5100`.
Swagger UI is available at `https://localhost:5101/swagger`.

---

## API Endpoints

* `GET    /api/popularIndicators?subscriptionType={type}`
* `GET    /api/projects?userIds=1,2,3`

Refer to Swagger UI for full request/response schemas.

---

## Testing

```bash
cd Tests/Queries
dotnet test
```
