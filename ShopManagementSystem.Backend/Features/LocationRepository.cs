using Dapper;
using ShopManagementSystem.Backend.Services;
using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features;

public class LocationRepository
{
    private readonly DapperContext _context;

    public LocationRepository(DapperContext context)
    {
        _context = context;
    }

    // Get all locations
    public async Task<IEnumerable<Location>> GetLocationsAsync()
    {
        var query = "SELECT * FROM Location";

        using (var connection = _context.CreateConnection())
        {
            var locations = await connection.QueryAsync<Location>(query);
            return locations.ToList();
        }
    }

    // Get location by Id
    public async Task<Location> GetLocationByIdAsync(int id)
    {
        var query = "SELECT * FROM Location WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QuerySingleOrDefaultAsync<Location>(query, new { Id = id });
        }
    }

    // Create new location
    public async Task CreateLocationAsync(Location location)
    {
        var query = "INSERT INTO Location (Name) VALUES (@Name)";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { location.Name });
        }
    }

    // Update a location
    public async Task UpdateLocationAsync(int id, Location location)
    {
        var query = "UPDATE Location SET Name = @Name WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { location.Name, Id = id });
        }
    }

    // Delete a location
    public async Task DeleteLocationAsync(int id)
    {
        var query = "DELETE FROM Location WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
