# Corporate CRM - Customer Module

Technical challenge implementation for a Customer Management module using DDD, CQRS and Event Sourcing principles.

---

## Tech Stack

- .NET 8 Web API
- React + TypeScript (Vite)
- PostgreSQL 16
- Docker & Docker Compose

---

## How to Run

Make sure Docker is installed.

From the project root:

docker compose up --build

---

## Endpoints

API Health Check:

http://localhost:8080/health

Frontend:

http://localhost:5173

---

## Services

- postgres -> Database
- api -> .NET 8 Web API
- web -> React Frontend

---

## Architecture

This project follows:

- Domain-Driven Design (DDD)
- CQRS (Command Query Responsibility Segregation)
- Event Sourcing (append-only event store)

Architectural decisions will be documented using ADR files.

---

## Development Notes

The entire environment runs inside Docker containers.
No local installation of .NET or Node is required.