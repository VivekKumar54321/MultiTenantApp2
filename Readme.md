MultiTenantApp2 ASP.NET Core  Web API
Features
•	Multi-Tenancy with strict data isolation (PostgreSQL database)
•	Microsoft Identity for authentication and user management
•	JWT-based secure authentication
•	CQRS pattern (separated commands & queries)
•	Clean Architecture
•	Full CRUD for Employee entity
•	Global Exception Handling
•	Entity Validation

Project Structure
•	MultiTenantApp2/
•	├── API/                   → Presentation layer (controllers, middleware, exceptions)
•	├── Application/           → Application layer (CQRS handlers, interfaces, DTOs)
•	├── Core/                  → Domain layer (Entities)
•	├── Infrastructure/        → Data access layer (EF Core, Identity, Services, Data, Migrations)
•	└── README.md              → Setup and documentation

Setup Instructions
•	1. Prerequisites:
	   .NET 8 SDK
	   PostgreSQL
	   pgAdmin
	   Visual Studio 2022 or newer
	   EF Core Tools: dotnet tool install --global dotnet-ef

•	2. PostgreSQL Configuration:
	   Create Database: CREATE DATABASE "MultiTenantDb";
	   Set Connection String in appsettings.json:
	 "DefaultConnection": "Host=localhost;Database=MultiTenantDb;Username=postgres;Password=postgres"




•	3. Run Migrations:

	# Add EF Core Tools
	dotnet tool install --global dotnet-ef
	dotnet add package Microsoft.EntityFrameworkCore.Design

	# Create Initial Migration
	dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
	dotnet ef database update --project Infrastructure --startup-project API


•	4. Seed Admin User & Default Tenant:
	Call DbSeeder.SeedDefaultTenantAndAdminAsync in Program.cs


Admin Credentials
•	Super Admin Email: admin@default.com.
•	Password: Admin@123
•	Default Tenant: Name ="Default Tenant”, Identifier="default-tenant”, Role= SuperAdmin; 


Example API Requests

1. Register Tenant & User
POST /api/Auth/register
Registers a new tenant and user.
Required fields: tenantIdentifier, tenantName, email, password

{
  "tenantIdentifier": "company-abc",
  "tenantName": "Company ABC",
  "email": "user@gmail.com",
  "password": "Admin@123"
}




2. Login
POST /api/Auth/login
Logs in user and returns JWT token.
Required fields: email, password

{
  "email": "user@gmail.com",
  "password": "Admin@123"

}

3. Create Employee
POST /api/Employees
Creates a new employee. Requires Bearer Token.
Fields: name, department
{
  "name": "Vivek Kumar",
  "department": "Computer Engineering"
}

4. Get All Employees
GET /api/Employees
Fetches all employees. Requires Bearer Token.

5. Get Employee by ID
GET /api/Employees/{id}
Fetches employee by ID. Requires Bearer Token.

6. Update Employee
PUT /api/Employees/{id}
Updates employee info. Requires Bearer Token.
Fields: name, department
{
  "name": "Vivek Thakur",
  "department": "IT"
}

7. Delete Employee
DELETE /api/Employees/{id}
Deletes employee by ID. Requires Bearer Token.



Authorization Notes
•	All employee endpoints require Authorization header with a valid JWT token.
•	Only users belonging to the same tenant can access their tenant's employees.

Vivek Thakur
Email: vivekkumarnxt@gmail.com
LinkedIn: linkedin.com/in/vivek-kumar-nxt
