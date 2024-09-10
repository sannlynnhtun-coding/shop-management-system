using Dapper;
using ShopManagementSystem.Backend.Services;
using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features;

public class StaffShopLinkRepository
{
    private readonly DapperContext _context;

    public StaffShopLinkRepository(DapperContext context)
    {
        _context = context;
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

    // Get all shops a staff member has access to
    public async Task<IEnumerable<Shop>> GetShopsByStaffIdAsync(int staffId)
    {
        var query = @"SELECT S.* 
                          FROM Shop S
                          INNER JOIN StaffShopLink SSL ON S.Id = SSL.ShopId
                          WHERE SSL.StaffId = @StaffId";

        using (var connection = _context.CreateConnection())
        {
            var shops = await connection.QueryAsync<Shop>(query, new { StaffId = staffId });
            return shops.ToList();
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
}
