using Dapper;
using ShopManagementSystem.Backend.Services;
using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features;

public class ShopRepository
{
    private readonly DapperContext _context;

    public ShopRepository(DapperContext context)
    {
        _context = context;
    }

    // Get all shops
    public async Task<IEnumerable<Shop>> GetShopsAsync()
    {
        var query = "SELECT * FROM Shop";

        using (var connection = _context.CreateConnection())
        {
            var shops = await connection.QueryAsync<Shop>(query);
            return shops.ToList();
        }
    }

    // Get shop by Id
    public async Task<Shop> GetShopByIdAsync(int id)
    {
        var query = "SELECT * FROM Shop WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QuerySingleOrDefaultAsync<Shop>(query, new { Id = id });
        }
    }

    // Create a new shop
    public async Task CreateShopAsync(Shop shop)
    {
        var query = "INSERT INTO Shop (Name, LocationId) VALUES (@Name, @LocationId)";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { shop.Name, shop.LocationId });
        }
    }

    // Update a shop
    public async Task UpdateShopAsync(int id, Shop shop)
    {
        var query = "UPDATE Shop SET Name = @Name, LocationId = @LocationId WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { shop.Name, shop.LocationId, Id = id });
        }
    }

    // Delete a shop
    public async Task DeleteShopAsync(int id)
    {
        var query = "DELETE FROM Shop WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
