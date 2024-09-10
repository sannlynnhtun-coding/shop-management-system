using Dapper;
using ShopManagementSystem.Backend.Services;
using ShopManagementSystem.Database.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopManagementSystem.Backend.Features;

public class StaffWithAccessibleShops
{
    public int StaffId { get; set; }
    public string StaffName { get; set; }
    public List<string> AccessibleShopList { get; set; }
    public string AccessibleShops { get; set; }
}

public class StaffWithShops
{
    public Staff Staff { get; set; }
    public List<Shop> Shops { get; set; }
}

public class StaffShopLinkRepository
{
    private readonly DapperContext _context;

    public StaffShopLinkRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffWithAccessibleShops>> GetStaffWithAccessibleShopsAsync()
    {
        var query = @"
                SELECT
                    ST.Id AS StaffId,
                    ST.Name AS StaffName,
                    STRING_AGG(S.Name, ', ') AS AccessibleShops
                FROM Staff ST
                LEFT JOIN StaffShopLink SSL ON ST.Id = SSL.StaffId
                LEFT JOIN Shop S ON SSL.ShopId = S.Id
                GROUP BY ST.Id, ST.Name;
            ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<StaffWithAccessibleShops>(query);
            result.ToList().ForEach(x =>
            {
                x.AccessibleShopList = x.AccessibleShops
                    .Split(",")
                    .Select(x => x.Trim())
                    .ToList();
            });
            return result;
        }
    }

    // Get all staff-shop links (for checking which staff has access to which shop)
    public async Task<IEnumerable<StaffShopLink>> GetAllStaffShopLinksAsync()
    {
        var query = "SELECT * FROM StaffShopLink";

        using (var connection = _context.CreateConnection())
        {
            var staffShopLinks = await connection.QueryAsync<StaffShopLink>(query);
            return staffShopLinks.ToList();
        }
    }

    // Grant access to a shop for a staff member
    public async Task GrantAccessToShopAsync(int staffId, int shopId)
    {
        var query = "INSERT INTO StaffShopLink (StaffId, ShopId) VALUES (@StaffId, @ShopId)";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { StaffId = staffId, ShopId = shopId });
        }
    }

    // Revoke access to a shop for a staff member
    public async Task RevokeAccessToShopAsync(int staffId, int shopId)
    {
        var query = "DELETE FROM StaffShopLink WHERE StaffId = @StaffId AND ShopId = @ShopId";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { StaffId = staffId, ShopId = shopId });
        }
    }

    // Check if a staff member has access to a specific shop
    public async Task<bool> HasAccessAsync(int staffId, int shopId)
    {
        var query = "SELECT COUNT(1) FROM StaffShopLink WHERE StaffId = @StaffId AND ShopId = @ShopId";

        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteScalarAsync<bool>(query, new { StaffId = staffId, ShopId = shopId });
        }
    }

    // Add multiple shop access for a staff member
    public async Task AddMultipleShopsForStaffAsync(int staffId, List<int> shopIds)
    {
        var query = "INSERT INTO StaffShopLink (StaffId, ShopId) VALUES (@StaffId, @ShopId)";

        using (var connection = _context.CreateConnection())
        {
            foreach (var shopId in shopIds)
            {
                // Check if this staff-shop link already exists before inserting
                var existsQuery = "SELECT COUNT(1) FROM StaffShopLink WHERE StaffId = @StaffId AND ShopId = @ShopId";
                var exists = await connection.ExecuteScalarAsync<bool>(existsQuery, new { StaffId = staffId, ShopId = shopId });

                if (!exists)
                {
                    await connection.ExecuteAsync(query, new { StaffId = staffId, ShopId = shopId });
                }
            }
        }
    }

    // Get all shops a staff member has access to
    public async Task<StaffWithShops> GetShopsByStaffIdAsync(int staffId)
    {
        var staffQuery = "SELECT Id, Name FROM Staff WHERE Id = @StaffId";
        var shopsQuery = @"
                SELECT S.Id, S.Name, S.LocationId 
                FROM Shop S
                INNER JOIN StaffShopLink SSL ON S.Id = SSL.ShopId
                WHERE SSL.StaffId = @StaffId";

        using (var connection = _context.CreateConnection())
        {
            // Fetch staff data
            var staff = await connection.QuerySingleOrDefaultAsync<Staff>(staffQuery, new { StaffId = staffId });

            if (staff == null)
            {
                return null; // No staff found
            }

            // Fetch the shops associated with the staff
            var shops = await connection.QueryAsync<Shop>(shopsQuery, new { StaffId = staffId });

            return new StaffWithShops
            {
                Staff = staff,
                Shops = shops.ToList()
            };
        }
    }

    // Get all staff and their corresponding shops
    public async Task<IEnumerable<dynamic>> GetAllStaffWithShopsAsync()
    {
        var query = @"
                SELECT ST.Id AS StaffId, ST.Name AS StaffName, 
                       S.Id AS ShopId, S.Name AS ShopName, S.LocationId
                FROM Staff ST
                LEFT JOIN StaffShopLink SSL ON ST.Id = SSL.StaffId
                LEFT JOIN Shop S ON SSL.ShopId = S.Id;
            ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<dynamic>(query);

            // Group staff and shops together
            var groupedResult = result.GroupBy(
                staff => new { staff.StaffId, staff.StaffName }, // Group by staff info
                staff => new { staff.ShopId, staff.ShopName, staff.LocationId } // Select shop info
            )
            .Select(group => new StaffWithShops
            {
                Staff = new Staff
                {
                    Id = group.Key.StaffId,
                    Name = group.Key.StaffName
                },
                Shops = group.Where(s => s.ShopId != null).Select(s => new Shop
                {
                    Id = s.ShopId,
                    Name = s.ShopName,
                    LocationId = s.LocationId
                }).ToList()
            })
            .ToList();

            return groupedResult;
        }
    }
}
