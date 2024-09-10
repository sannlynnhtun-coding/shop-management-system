namespace ShopManagementSystem.Backend.Features;

public static class StaffShopLinkEndpoints
{
    public static void AddStaffShopLinkEndpoints(this WebApplication app)
    {
        var staffShopLinkEndpoint = app.MapGroup("/staff-shop-links").WithTags("Staff Shop Links");

        // Get staff members and the shops they can access
        staffShopLinkEndpoint.MapGet("/staff-shop-access", async (StaffShopLinkRepository repository) =>
        {
            var staffAccess = await repository.GetStaffWithAccessibleShopsAsync();
            return Results.Ok(staffAccess);
        });

        // Get all staff-shop access links
        staffShopLinkEndpoint.MapGet("/", async (StaffShopLinkRepository repository) =>
        {
            var staffShopLinks = await repository.GetAllStaffShopLinksAsync();
            return Results.Ok(staffShopLinks);
        });

        // Get all staff with their corresponding shops
        staffShopLinkEndpoint.MapGet("/all", async (StaffShopLinkRepository repository) =>
        {
            var staffWithShops = await repository.GetAllStaffWithShopsAsync();
            return Results.Ok(staffWithShops);
        });


        // Grant a staff member access to a shop
        staffShopLinkEndpoint.MapPost("/{staffId}/{shopId}", async (int staffId, int shopId, StaffShopLinkRepository repository) =>
        {
            var hasAccess = await repository.HasAccessAsync(staffId, shopId);
            if (hasAccess)
            {
                return Results.Conflict("Staff member already has access to this shop.");
            }

            await repository.GrantAccessToShopAsync(staffId, shopId);
            return Results.Created($"/{staffId}/{shopId}", new { StaffId = staffId, ShopId = shopId });
        });

        // Revoke a staff member's access to a shop
        staffShopLinkEndpoint.MapDelete("/{staffId}/{shopId}", async (int staffId, int shopId, StaffShopLinkRepository repository) =>
        {
            var hasAccess = await repository.HasAccessAsync(staffId, shopId);
            if (!hasAccess)
            {
                return Results.NotFound("Staff member does not have access to this shop.");
            }

            await repository.RevokeAccessToShopAsync(staffId, shopId);
            return Results.NoContent();
        });

        // Check if a staff member has access to a specific shop
        staffShopLinkEndpoint.MapGet("/{staffId}/{shopId}", async (int staffId, int shopId, StaffShopLinkRepository repository) =>
        {
            var hasAccess = await repository.HasAccessAsync(staffId, shopId);
            return hasAccess ? Results.Ok() : Results.NotFound();
        });

        // Add multiple shop access for a staff member
        staffShopLinkEndpoint.MapPost("/{staffId}/multiple-shops", async (int staffId, List<int> shopIds, StaffShopLinkRepository repository) =>
        {
            if (shopIds == null || !shopIds.Any())
            {
                return Results.BadRequest("No shopIds provided.");
            }

            await repository.AddMultipleShopsForStaffAsync(staffId, shopIds);
            return Results.Ok(new { Message = "Shop access added successfully", StaffId = staffId, ShopIds = shopIds });
        });

        // Get multiple shops for a staff member
        staffShopLinkEndpoint.MapGet("/{staffId}/multiple-shops", async (int staffId, StaffShopLinkRepository repository) =>
        {
            var result = await repository.GetShopsByStaffIdAsync(staffId);

            if (!result.Shops.Any())
            {
                return Results.NotFound(new { Message = "No shops found for this staff member." });
            }

            return Results.Ok(result);
        });
    }
}
