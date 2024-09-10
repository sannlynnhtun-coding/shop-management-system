using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features
{
    public static class LocationEndpoints
    {
        public static void AddLocationEndpoint(this WebApplication app)
        {
            var locationEndpoint = app.MapGroup("/locations").WithTags("Locations");

            // Get all locations
            locationEndpoint.MapGet("/", async (LocationRepository repository) =>
            {
                var locations = await repository.GetLocationsAsync();
                return Results.Ok(locations);
            });

            // Get location by Id
            locationEndpoint.MapGet("/{id}", async (int id, LocationRepository repository) =>
            {
                var location = await repository.GetLocationByIdAsync(id);
                return location is not null ? Results.Ok(location) : Results.NotFound();
            });

            // Create a new location
            locationEndpoint.MapPost("/", async (Location location, LocationRepository repository) =>
            {
                await repository.CreateLocationAsync(location);
                return Results.Created($"/{location.Id}", location);
            });

            // Update a location
            locationEndpoint.MapPut("/{id}", async (int id, Location location, LocationRepository repository) =>
            {
                var existingLocation = await repository.GetLocationByIdAsync(id);
                if (existingLocation is null)
                {
                    return Results.NotFound();
                }

                await repository.UpdateLocationAsync(id, location);
                return Results.NoContent();
            });

            // Delete a location
            locationEndpoint.MapDelete("/{id}", async (int id, LocationRepository repository) =>
            {
                var location = await repository.GetLocationByIdAsync(id);
                if (location is null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteLocationAsync(id);
                return Results.NoContent();
            });
        }
    }
}
