# BankingAppAPI

A banking application with a .NET 10 Minimal API backend.

## Key features

- Customer management: create, read, update, delete customers.
- Account management: create, read, update, delete accounts.
- Relationship handling: accounts are linked to customers.
- Validation rules: data annotations enforce required values and value ranges.
- Automated database creation: SQLite database is created automatically at startup.

## Tech stack

- .NET 10
- ASP.NET Core minimal APIs
- Entity Framework Core
- SQLServer
- Swagger / OpenAPI

## Project structure

- `BankingApi.API/`
  - `Data/` — database context setup.
  - `Interfaces/` — service abstraction definitions.
  - `Model/` — domain entities with validation.
  - `Services/` — business logic and persistence behavior.
  - `Program.cs` — startup configuration, DI, routing, and middleware.
  - `appsettings.json` — runtime configuration.


## API endpoints

- `GET /` — root status with endpoint list.
- `GET /health` — health check.
- `GET /customers` — list all customers.
- `POST /customers` — add a new customer.
- `PUT /customers/{id}` — update a customer.
- `DELETE /customers/{id}` — remove a customer.
- `GET /accounts` — list all accounts.
- `GET /accounts/{id}` — get account details.
- `POST /accounts` — add a new account.
- `PUT /accounts/{id}` — update an account.
- `DELETE /accounts/{id}` — delete an account.

## Running the API locally

1. Navigate to the API folder:
   ```bash
   cd BankingApi.API
   ```
2. Run the app:
   ```bash
   dotnet run
   ```
3. Open Swagger UI in the browser during development:
   ```
   http://localhost:5000/swagger
   ```

## What this project demonstrates

- Strong understanding of backend development with .NET and service-oriented design.
- Ability to implement validation, error handling, and API documentation.
- Practical use of EF Core for persistence and database migrations.
- Experience working with backend application repositories.
- Attention to maintainable code organization and developer-friendly project structure.
