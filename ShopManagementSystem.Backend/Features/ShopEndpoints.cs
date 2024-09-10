using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features;

public static class ShopEndpoints
{
    public static void AddShopEndpoint(this WebApplication app)
    {
        // Get all shops
        app.MapGet("/shops", async (ShopRepository repository) =>
        {
            var shops = await repository.GetShopsAsync();
            return Results.Ok(shops);
        });

        // Get shop by Id
        app.MapGet("/shops/{id}", async (int id, ShopRepository repository) =>
        {
            var shop = await repository.GetShopByIdAsync(id);
            return shop is not null ? Results.Ok(shop) : Results.NotFound();
        });

        // Create a new shop
        app.MapPost("/shops", async (Shop shop, ShopRepository repository) =>
        {
            await repository.CreateShopAsync(shop);
            return Results.Created($"/shops/{shop.Id}", shop);
        });

        // Update an existing shop
        app.MapPut("/shops/{id}", async (int id, Shop shop, ShopRepository repository) =>
        {
            var existingShop = await repository.GetShopByIdAsync(id);
            if (existingShop is null)
            {
                return Results.NotFound();
            }

            await repository.UpdateShopAsync(id, shop);
            return Results.NoContent();
        });

        // Delete a shop
        app.MapDelete("/shops/{id}", async (int id, ShopRepository repository) =>
        {
            var shop = await repository.GetShopByIdAsync(id);
            if (shop is null)
            {
                return Results.NotFound();
            }

            await repository.DeleteShopAsync(id);
            return Results.NoContent();
        });
    }
}
