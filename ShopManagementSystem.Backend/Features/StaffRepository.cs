using Dapper;
using ShopManagementSystem.Backend.Services;
using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features;

public class StaffRepository
{
    private readonly DapperContext _context;

    public StaffRepository(DapperContext context)
    {
        _context = context;
    }

    // Get all staff
    public async Task<IEnumerable<Staff>> GetStaffAsync()
    {
        var query = "SELECT * FROM Staff";

        using (var connection = _context.CreateConnection())
        {
            var staff = await connection.QueryAsync<Staff>(query);
            return staff.ToList();
        }
    }

    // Get staff by Id
    public async Task<Staff> GetStaffByIdAsync(int id)
    {
        var query = "SELECT * FROM Staff WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QuerySingleOrDefaultAsync<Staff>(query, new { Id = id });
        }
    }

    // Create a new staff
    public async Task CreateStaffAsync(Staff staff)
    {
        var query = "INSERT INTO Staff (Name) VALUES (@Name)";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { staff.Name });
        }
    }

    // Update an existing staff
    public async Task UpdateStaffAsync(int id, Staff staff)
    {
        var query = "UPDATE Staff SET Name = @Name WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { staff.Name, Id = id });
        }
    }

    // Delete a staff
    public async Task DeleteStaffAsync(int id)
    {
        var query = "DELETE FROM Staff WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
