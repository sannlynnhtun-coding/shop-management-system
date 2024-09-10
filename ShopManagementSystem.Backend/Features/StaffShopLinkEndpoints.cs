namespace ShopManagementSystem.Backend.Features;

public static class StaffShopLinkEndpoints
{
    public static void AddStaffShopLinkEndpoints(this WebApplication app)
    {
        // Get all staff-shop access links
        app.MapGet("/staff-shop-links", async (StaffShopLinkRepository repository) =>
        {
            var staffShopLinks = await repository.GetAllStaffShopLinksAsync();
            return Results.Ok(staffShopLinks);
        });

        // Get all shops a staff member has access to
        app.MapGet("/staff-shop-links/{staffId}/shops", async (int staffId, StaffShopLinkRepository repository) =>
        {
            var shops = await repository.GetShopsByStaffIdAsync(staffId);
            return shops.Any() ? Results.Ok(shops) : Results.NotFound();
        });

        // Grant a staff member access to a shop
        app.MapPost("/staff-shop-links/{staffId}/{shopId}", async (int staffId, int shopId, StaffShopLinkRepository repository) =>
        {
            var hasAccess = await repository.HasAccessAsync(staffId, shopId);
            if (hasAccess)
            {
                return Results.Conflict("Staff member already has access to this shop.");
            }

            await repository.GrantAccessToShopAsync(staffId, shopId);
            return Results.Created($"/staff-shop-links/{staffId}/{shopId}", new { StaffId = staffId, ShopId = shopId });
        });

        // Revoke a staff member's access to a shop
        app.MapDelete("/staff-shop-links/{staffId}/{shopId}", async (int staffId, int shopId, StaffShopLinkRepository repository) =>
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
        app.MapGet("/staff-shop-links/{staffId}/{shopId}", async (int staffId, int shopId, StaffShopLinkRepository repository) =>
        {
            var hasAccess = await repository.HasAccessAsync(staffId, shopId);
            return hasAccess ? Results.Ok() : Results.NotFound();
        });
    }
}
