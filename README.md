### Project Description: **Shop Management System API**

#### Overview:
The **Shop Management System API** is developed using ASP.NET Core 8 Minimal API, integrated with Dapper ORM, to streamline the management of shops, staff, and access control between them. This API enables the creation and management of shops and staff members, along with handling which staff members have access to specific shops through a many-to-many relationship. The API also supports assigning multiple shops to staff members in one request. All endpoints are neatly categorized and documented using Swagger UI for easy exploration and testing.

#### Key Features:
- **Staff Management**: Manage staff members with either global access or restricted access to specific shops.
- **Shop Management**: Create and manage shops, which are linked to specific locations.
- **Staff-Shop Links**: Define which staff members can access specific shops through a many-to-many relationship.
- **Multiple Shop Assignment**: Assign multiple shops to a staff member in one API request, optimizing management of shop access.
- **Data Retrieval**: Retrieve detailed information about staff members, shops, and their access relationships in a structured way.
- **Swagger UI Documentation**: The API is fully documented using Swagger UI, providing a user-friendly interface for testing and exploring endpoints.

#### Technologies Used:
- **ASP.NET Core 8**: Framework for creating modern web applications, used here to build the minimal API endpoints.
- **Dapper ORM**: A lightweight ORM for efficient database interaction with Microsoft SQL Server.
- **Swagger UI**: Automatic API documentation and testing interface.
- **Microsoft SQL Server**: The relational database for storing staff, shop, and access control information.
- **MapGroup() in Minimal API**: Grouped routes for better organization and easier management of related API endpoints.

#### Key Endpoints:
1. **Staff Endpoints** (`/staff`):
   - Retrieve all staff members.
   - Manage staff members and their access permissions, whether global or limited to specific shops.

2. **Shop Endpoints** (`/shops`):
   - Retrieve all shops.
   - Manage shop details, including their names and locations.

3. **Staff-Shop Links Endpoints** (`/staff-shop-links`):
   - Retrieve the list of shops a specific staff member can access.
   - Assign multiple shops to a staff member in a single API request.
   - Retrieve all staff members along with the shops they can access.

#### Example Endpoints:
1. **Retrieve All Staff**: `GET /staff`
   - Returns a list of all staff members currently in the system.

2. **Retrieve All Shops**: `GET /shops`
   - Returns a list of all shops currently in the system.

3. **Assign Multiple Shops to a Staff Member**: `POST /staff-shop-links/{staffId}/multiple-shops`
   - Assigns multiple shops to a staff member in a single API call, streamlining the process.

4. **Get All Staff and Their Accessible Shops**: `GET /staff-shop-links/all`
   - Returns all staff members along with the shops they are allowed to access.

#### API Documentation:
All endpoints are documented using **Swagger UI**, allowing users to interact with and test each API. Each endpoint is grouped based on functionality for better organization:
- **Staff**: Endpoints related to managing staff members.
- **Shops**: Endpoints related to managing shops.
- **Staff-Shop Links**: Endpoints that handle the relationships between staff members and shops, including access control.

#### Usage:
The **Shop Management System API** is perfect for businesses that manage multiple shops and need to control which staff members have access to specific locations. It provides a clear, efficient way to manage permissions, track relationships between staff and shops, and keep the system organized.

---

### Database Setup

To scaffold the database and set up your project, follow these steps:

1. **Entity Framework Command**: Use the following command to scaffold the database context and models:

```bash
dotnet ef dbcontext scaffold "Server=.;Database=ShopManagementSystem;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext -f
```

This will create the necessary models and the `AppDbContext` for interacting with the SQL Server database.

2. **SQL Server Setup**: Run the following SQL script to set up the database structure.

```sql
-- Create the database
create database ShopManagementSystem 
go

-- Switch to the new database
use ShopManagementSystem
go

-- Create Location table
create table dbo.Location
(
    Id   int identity primary key,
    Name nvarchar(100) not null
)
go

-- Create Shop table
create table dbo.Shop
(
    Id         int identity primary key,
    Name       nvarchar(100) not null,
    LocationId int
)
go

-- Create Staff table
create table dbo.Staff
(
    Id   int identity primary key,
    Name nvarchar(100) not null
)
go

-- Create Staff-Shop link table to manage access relationships
create table dbo.StaffShopLink
(
    StaffId int not null,
    ShopId  int not null,
    primary key (StaffId, ShopId)
)
go
```

This script creates the required tables (`Location`, `Shop`, `Staff`, and `StaffShopLink`) in the `ShopManagementSystem` database.

By following these steps, you will have a fully operational API and a database schema ready for managing staff, shops, and their access relationships.