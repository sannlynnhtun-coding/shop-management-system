```
dotnet ef dbcontext scaffold "Server=.;Database=ShopManagementSystem;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext -f

```

```sql

-- Create Database
CREATE DATABASE ShopManagement;

-- Use the Database
USE ShopManagement;

-- Create Location Table
CREATE TABLE Location (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Create Shop Table
CREATE TABLE Shop (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    LocationId INT,
    FOREIGN KEY (LocationId) REFERENCES Location(Id)
);

-- Create Staff Table
CREATE TABLE Staff (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    HasGlobalAccess BIT NOT NULL DEFAULT 0
);

-- Create StaffShopLink Table to manage limited shop access
CREATE TABLE StaffShopLink (
    StaffId INT,
    ShopId INT,
    FOREIGN KEY (StaffId) REFERENCES Staff(Id),
    FOREIGN KEY (ShopId) REFERENCES Shop(Id),
    PRIMARY KEY (StaffId, ShopId)
);

```