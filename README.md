# Corporate CRM - Customer Module

Technical challenge implementation for a Customer Management module
using DDD, CQRS and Event Sourcing principles.

------------------------------------------------------------------------

## Tech Stack

-   .NET 8 Web API
-   React + TypeScript (Vite)
-   PostgreSQL 16
-   Docker & Docker Compose
-   xUnit + FluentAssertions (Backend Tests)
-   Cypress (Frontend E2E Tests)

------------------------------------------------------------------------

## How to Run

Make sure Docker is installed.

From the project root:

```
docker compose up --build
```

------------------------------------------------------------------------

## Application URLs

### Frontend

http://localhost:5173

### Backend API

http://localhost:8080

### Swagger (Development Mode)

http://localhost:8080/swagger

### Health Check

GET http://localhost:8080/health

------------------------------------------------------------------------

# API Endpoints

## Postal Codes

### Get address by CEP

GET /postal-codes/{cep}

Example: GET http://localhost:8080/postal-codes/11065-651

------------------------------------------------------------------------

## Customers

### Create Customer

POST /customers

Example payload:

```
{ "customerType": "Individual", "name": "Nelson", "cpfCnpj":
"39940838859", "birthOrFoundationDate": "1990-06-13", "email":
"nelson@email.com", "phone": "11999999999", "postalCode": "11065651",
"street": "Rua X", "city": "Santos", "state": "SP", "performedBy":
"admin" }
```

Success response: 201 Created

Validation error example:

```
{ "message": "Validation failed", "errors": \[ { "propertyName":
"Email", "errorMessage": "E-mail já cadastrado" } \] }
```

Conflict example: 409 Conflict { "message": "E-mail já cadastrado" }

------------------------------------------------------------------------

# Backend Tests

Run backend tests:

```
docker compose exec api dotnet test
```

------------------------------------------------------------------------

# Frontend E2E Tests (Cypress)

Run Cypress (headless):

```
docker compose exec web npm run cy:run
```

Run Cypress (interactive):

```
docker compose exec web npm run cy:open
```

------------------------------------------------------------------------

# Architecture

This project follows:

-   Domain-Driven Design (DDD)
-   CQRS (Command Query Responsibility Segregation)
-   Event Sourcing (append-only event store)

## Layers

-   CorporateCrm.Domain
-   CorporateCrm.Application
-   CorporateCrm.Infrastructure
-   CorporateCrm.Api

------------------------------------------------------------------------
