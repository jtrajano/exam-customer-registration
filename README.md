# Customer Registration & Onboarding System

A self-contained Customer Registration system built with .NET 10 (Clean Architecture) and React (Vite).

## Features

- **Clean Architecture**: Decoupled layers (Domain, Application, Infrastructure, Api).
- **Backend**: .NET 10 Minimal APIs with SQLite & EF Core.
- **Frontend**: React 18 with TypeScript, Vite, and Vanilla CSS.
- **Logging**: Structured logging with Serilog (Console & File).
- **Error Handling**: Global exception handling with RFC-compliant Problem Details.
- **API Documentation**: Integrated Scalar API reference.
- **Testing**: Unit and Integration tests with xUnit, Moq, and FluentAssertions.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js (v18+)](https://nodejs.org/) & npm

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd CustomerRegistration
```

### 2. Run the Application
The solution is configured to run both the API and the React frontend (via SPA proxy) with a single command.

```bash
dotnet run --project src/CustomerRegistration.Api
```

- **API/Frontend**: `https://localhost:5001` (or the port shown in your terminal)
- **API Documentation (Scalar)**: `https://localhost:5001/scalar/v1`

### 3. Run Tests
```bash
dotnet test
```

## Project Structure

- `src/CustomerRegistration.Api`: Entry point, endpoints, and middleware.
- `src/CustomerRegistration.Application`: Business logic, DTOs, and services.
- `src/CustomerRegistration.Domain`: Core entities and repository interfaces.
- `src/CustomerRegistration.Infrastructure`: Data access (EF Core), SQLite configuration, and repository implementations.
- `CustomerRegistration.Client`: React frontend application.
- `src/CustomerRegistration.Test`: Unit and Integration tests.

## Logging

Logs are written to:
- **Console**: Structured output for development.
- **File**: `src/CustomerRegistration.Api/logs/log-.txt` (rolling daily).

## Architecture Decisions

- **SQLite**: Chosen for its "zero-configuration" requirement, making the system fully self-contained.
- **SPA Proxy**: Simplifies development by allowing `dotnet run` to manage both the backend and the frontend dev server.
- **Serilog**: Provides robust, structured logging that is easy to sink to various destinations.
- **Vanilla CSS**: Used for the frontend to minimize external dependencies and ensure maximum flexibility.
