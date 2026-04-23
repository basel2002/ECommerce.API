# ApiFinalProject

![.NET](https://img.shields.io/badge/.NET-9-informational)
![C#](https://img.shields.io/badge/C%23-10-blue)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8-lightgrey)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-darkgreen)
![JWT](https://img.shields.io/badge/Auth-JWT-yellow)

## Project Title & Description

`ApiFinalProject` is an e-commerce backend API that implements product and category management, shopping cart operations and user authentication using ASP.NET Core (Identity + JWT). It provides endpoints for products, categories and carts and uses a layered architecture (API / BLL / DAL / Common) with Entity Framework Core as the ORM.

Purpose: provide a minimal but extensible foundation for an e-commerce REST API with role-based authorization (admin/user), pagination and filter support for product listing.

## Tech Stack

- Language: `C#` targeting `.NET 9`
- Web framework: `ASP.NET Core` (Web API)
- Authentication: `ASP.NET Core Identity` + `JWT (Bearer)`
- ORM: `Entity Framework Core` with SQL Server
- Project structure: separate `ApiFinalProject` (web), `BLL`, `DAL`, and `Common` projects
- Tooling: `dotnet` CLI, Visual Studio

## Project Structure

Top-level folders and purpose:

- `ApiFinalProject/` — Web API project (controllers, `Program.cs`, `appsettings`)
  - `Controllers/` — API controllers: `authController`, `ProductsController`, `CategoriesController`, `CartsController`
  - `appsettings.json`, `appsettings.Development.json` — configuration (connection string, JWT settings in Development)
- `DAL/` — Data access layer
  - `Data/Context/AppDbContext.cs` — EF `DbContext`
  - `Data/Models/` — EF entities (e.g. `Product`, `Categorie`, `Cart`, `CartItem`, `Order`, `OrderItem`, `ApplicationUser`)
  - `Repositories/` — repository implementations and generic repository
  - `Migrations/` — EF Migrations (schema evolution)
- `BLL/` — Business logic layer
  - `Manager/` — managers (e.g. `ProductManager`, `CategoryManager`, `CartManager`)
  - `DTOs/` — request/response DTOs (Product, Category, Cart, User)
  - `Validation/` — FluentValidation validators
  - `Settings/` — e.g. `JwtSettings`
- `Common/` — shared utilities
  - `Filtering/`, `Pagination/`, `General Result/` — paging types and `GeneralResult` wrapper

Important files:
- `ApiFinalProject/Program.cs` — DI configuration, JWT authentication, authorization policies, OpenAPI registration
- `ApiFinalProject/appsettings.Development.json` — sample connection string and `JwtSettings`
- `DAL/Migrations/` — existing migrations to create schema

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (local or remote)
- Optional: Visual Studio 2022/2026 or VS Code

### Installation steps

1. Clone the repository and open solution in Visual Studio or via CLI:

```bash
git clone https://github.com/basel2002/ECommerce.API.git
cd ApiFinalProject
```

2. Restore and build:

```bash
dotnet restore
dotnet build
```

3. Configure the database connection and JWT (see Configuration below).

4. Apply migrations to create the database:

```bash
cd DAL
dotnet ef database update --project DAL --startup-project ApiFinalProject
```

5. Run the API:

```bash
cd ApiFinalProject
dotnet run
```

API will be available at the configured `applicationUrl` (see `Properties/launchSettings.json`).

### Configuration

Configuration is read from `appsettings.json` / `appsettings.Development.json` and environment variables.

Key settings in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "E-Commerce": "Server=.;DataBase=E-Commerce-DB;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "Issuer": "CompanySystem",
    "Audience": "CompanySystemClient",
    "DurationInMinutes": 600,
    "SecretKey": "JVNg4ijfYm+q7Ekx3jiMcMFggctux65zgipeFpV+o1I="
  }
}
```

- Connection string key: `ConnectionStrings:E-Commerce` (used by DAL service registration).
- JWT settings key: `JwtSettings` (Issuer, Audience, DurationInMinutes, SecretKey). The `SecretKey` is a Base64-encoded symmetric key.

You may also set these values via environment variables (for production) or override using user secrets.

## Database Setup

Migrations are provided in `DAL/Migrations`. To create or update the database use the `dotnet ef` tooling pointing to the DAL project and using `ApiFinalProject` as the startup project (so configuration and `Program.cs` are available):

```bash
# from repository root
dotnet ef database update --project DAL --startup-project ApiFinalProject
```

Note: There is no project-level seeding implemented. You can add seeding in `AppDbContext.OnModelCreating` or in an initialization routine.

## API Endpoints

All responses are wrapped with `Common.GeneralResult` or `Common.GeneralResult<T>` which includes `Success`, `Message`, optional `Errors`, and `Data` when applicable.

Base URL example: `https://localhost:7188` or `http://localhost:5128` depending on launch settings.

1) Authentication

- POST `/api/auth/Login`
  - Description: Authenticate user and return JWT token
  - Request body (`LoginDto`):

```json
{
  "email": "user@example.com",
  "password": "P@ssw0rd",
  "rememberMe": false
}
```

  - Response (success): `TokenDto` wrapped in `Ok` (example):

```json
{
  "accessToken": "<jwt-token>",
  "durationInMinutes": 600,
  "tokenType": "Bearer"
}
```

- POST `/api/auth/Register`
  - Description: Create new user and assign `user` role
  - Request body (`RegisterDto`):

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "userName": "john_doe",
  "password": "P@ssw0rd"
}
```

  - Response: success message or validation errors (wrapped in `GeneralResult`).

2) Products

- GET `/api/products` — Get all products
  - Response: `GeneralResult<IEnumerable<ProductReadDto>>`
  - Example `ProductReadDto`:

```json
{
  "id": 1,
  "name": "Product name",
  "price": 10.5,
  "description": "...",
  "imageUrl": "https://...",
  "stockQty": 100,
  "category": "Category name"
}
```

- GET `/api/products/{id}` — Get product details by id
  - Response: `GeneralResult<ProductReadDto>`

- POST `/api/products` — Create product (Admin only)
  - Request (`ProductCreateDto`):

```json
{
  "name": "New product",
  "price": 9.99,
  "description": "...",
  "imageUrl": "https://..",
  "stockQty": 10,
  "categoryId": 1
}
```

  - Response: `GeneralResult<ProductReadDto>` (created product)

- PUT `/api/products/{id}` — Update product (Admin only)
  - Request (`ProductEditDto`):

```json
{
  "name": "Updated",
  "price": 8.5,
  "description": "...",
  "imageUrl": "https://..",
  "stockQty": 5
}
```

- DELETE `/api/products/{id}` — Delete product (Admin only)

- GET `/api/products/Pagination` — Get paginated list of products
  - Query params: `pageNumber`, `pageSize`, `search` and `categoryId` (for `ProductFilterParameters`).
  - Response: `GeneralResult<PageResult<ProductReadDto>>` with `Metadata` (current page, page size, total pages, total count).

3) Categories

- GET `/api/categories` — Get all categories
  - Response: `GeneralResult<IEnumerable<CategoryReadDto>>` (`CategoryReadDto` has `id`, `name`, `imageUrl`)

- GET `/api/categories/{id}` — Get category details

- POST `/api/categories` — Create category (Admin only)
  - Request (`CategoryCreateDto`):

```json
{
  "name": "Electronics",
  "description": "...",
  "imageUrl": "https://..."
}
```

- PUT `/api/categories/{id}` — Update category (Admin only)
  - Request (`CategoryEditDto`)

- DELETE `/api/categories/{id}` — Delete category (Admin only)

4) Cart

- POST `/api/carts` — Add product to authenticated user's cart (Requires auth)
  - Request body (`CartAddToDto`):

```json
{
  "productId": 1
}
```

- GET `/api/carts` — Get authenticated user's cart (Requires `user` role)
  - Response: `GeneralResult<IEnumerable<CartItemReadDto>>` where `CartItemReadDto` contains `productName`, `quantity`, `unitPrice`.

- DELETE `/api/carts/{id}` — Remove cart item by id (Requires `user` role)

- PUT `/api/carts` — Update cart item quantity (Requires `user` role)
  - Request (`CartItemUpdateDto`):

```json
{
  "productId": 1,
  "newQuantity": 3
}
```

## Features

- Layered architecture: API, Business (BLL), Data (DAL), Common utilities
- Authentication using ASP.NET Identity and JWT tokens
- Role-based authorization policies: `AdminOnly`, `UserOnly`
- Product pagination and filtering (search, category)
- Standardized API responses using `GeneralResult<T>` wrapper
- EF Core migrations included to create the initial schema

## Authentication

- Identity stores users in `AspNetUsers` tables (managed by `ApplicationUser` class).
- Register endpoint adds new user and assigns the `user` role.
- Login endpoint returns a `TokenDto` with a JWT token. The token includes standard claims (`NameIdentifier`, `Name`, `Email`) and user roles (added as `ClaimTypes.Role`).
- JWT configuration is read from `JwtSettings` in configuration. The token is validated using `JwtBearer` middleware.
- Authorization policies are configured in `Program.cs`:
  - `AdminOnly` requires role `admin`
  - `UserOnly` requires role `user`

Notes: There is no automatic role seeding in the repository; you must create roles (`admin`, `user`) in the database or via an initialization routine.

## Error Handling

- The project uses `Common.GeneralResult` and `Common.GeneralResult<T>` consistently to return operation results.
- On validation errors, managers use FluentValidation and map errors to a structured `Errors` dictionary inside `GeneralResult`.
- Controllers return appropriate HTTP status codes:
  - `200 OK` for success
  - `400 Bad Request` for invalid requests
  - `401 Unauthorized` when authentication fails
  - `404 Not Found` when resources are missing

Example error response:

```json
{
  "success": false,
  "message": "Failed",
  "errors": {
    "Name": [ { "code": "NotEmpty", "description": "Name is required" } ]
  }
}
```

## System Design & Entity Relationship Diagram

Entities and relationships (simplified):

- `ApplicationUser` (1) — (1) `Cart`
- `Cart` (1) — (M) `CartItem`
- `Product` (1) — (M) `CartItem`
- `Categorie` (1) — (M) `Product`
- `ApplicationUser` (1) — (M) `Order`
- `Order` (1) — (M) `OrderItem`
- `Product` (1) — (M) `OrderItem`

ASCII ER Diagram:

```
+---------------------+         +----------------+
| ApplicationUser     |1       1| Cart           |
|---------------------|---------|----------------|
| Id                  |         | Id             |
| FirstName           |         | UserId (FK)    |
| LastName            |         +---^------------+
+---------------------+             |
      |1                              |1
      |                               |
      |                               |
      |                               | M
      |                               v
      |                           +----------------+
      |                           | CartItem       |
      |                           |----------------|
      |                           | Id             |
      |                           | CartId (FK)    |
      |                           | ProductId (FK) |
      |                           | Quantity       |
      |                           +----------------+
      |                                   ^
      |                                   |
      |                                   | M
      |                                   |
      |                               +-----------+
      |                               | Product   |
      |                               |-----------|
      |                               | Id        |
      |                               | Name      |
      |                               | Price     |
      |                               | CategorieId (FK)
      |                               +-----^-----+
      |                                     |
      |                                     | M
      v                                     |
+----------------+
| Order          |         +----------------+
|----------------|1      M| OrderItem      |
| Id             |--------|----------------|
| UserId (FK)    |        | Id             |
+----------------+        | OrderId (FK)   |
                          | ProductId (FK)  |
                          +----------------+

+----------------+
| Categorie      |
|----------------|
| Id             |
| Name           |
+----------------+
```

This diagram reflects the entity classes and foreign keys represented in `DAL.Data.Models` and the EF migrations.

## Contributing

- Fork the repository and create feature branches.
- Follow the existing layered structure: keep controllers thin, place business rules in BLL managers and data access in DAL repositories.
- Add unit/integration tests where appropriate.
- Create EF migrations under `DAL` and deliver them with PRs if schema changes are required.
- Run `dotnet format` and ensure solution builds:

```bash
dotnet restore
dotnet build
```

## License

No `LICENSE` file is present in the repository. Add a `LICENSE` file to make the project's license explicit (for example `MIT` or another permissive license).


If you want, I can also:
- Add a small role-and-data seeding step to `Program.cs` to create `admin` and `user` roles and a sample admin user.
- Add example HTTP requests for each endpoint in a Postman collection or `http` file.
- Generate a visual diagram (PNG/SVG) for the ERD.

