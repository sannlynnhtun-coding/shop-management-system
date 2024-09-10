```
dotnet ef dbcontext scaffold "Server=.;Database=ShopManagementSystem;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext -f

```

```sql

create database ShopManagementSystem 
go

use ShopManagementSystem
go

create table dbo.Location
(
    Id   int identity
        primary key,
    Name nvarchar(100) not null
)
go

create table dbo.Shop
(
    Id         int identity
        primary key,
    Name       nvarchar(100) not null,
    LocationId int
)
go

create table dbo.Staff
(
    Id   int identity
        primary key,
    Name nvarchar(100) not null,
)
go

create table dbo.StaffShopLink
(
    StaffId int not null,
    ShopId  int not null,
    primary key (StaffId, ShopId)
)
go


```