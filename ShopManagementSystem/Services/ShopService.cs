using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Services;

public class ShopService
{
    private readonly AppDbContext _context;

    public ShopService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Shop>> GetShopsForStaffAsync(int staffId)
    {
        var staff = await _context.Staff
            .Include(s => s.StaffShopLinks)
            .ThenInclude(sl => sl.Shop)
            .FirstOrDefaultAsync(s => s.Id == staffId);

        if (staff == null)
        {
            throw new KeyNotFoundException("Staff not found.");
        }

        if (staff.HasGlobalAccess)
        {
            return await _context.Shops
                .Include(s => s.Location)
                .ToListAsync();
        }

        return staff.StaffShopLinks.Select(sl => sl.Shop).ToList();
    }

    public async Task AddShopAccessForStaffAsync(int staffId, int shopId)
    {
        var staff = await _context.Staff.FindAsync(staffId);
        var shop = await _context.Shops.FindAsync(shopId);

        if (staff == null || shop == null)
        {
            throw new KeyNotFoundException("Staff or Shop not found.");
        }

        var staffShopLink = new StaffShopLink
        {
            StaffId = staffId,
            ShopId = shopId
        };

        _context.StaffShopLinks.Add(staffShopLink);
        await _context.SaveChangesAsync();
    }
}
