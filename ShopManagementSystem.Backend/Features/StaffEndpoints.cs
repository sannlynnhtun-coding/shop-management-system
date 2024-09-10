using ShopManagementSystem.Database.Models;

namespace ShopManagementSystem.Backend.Features;

public static class StaffEndpoints
{
    public static void AddStaffEndpoint(this WebApplication app)
    {
        var staffEndpoint = app.MapGroup("/staffs").WithTags("Staffs");

        // Get all staff
        staffEndpoint.MapGet("/", async (StaffRepository repository) =>
        {
            var staff = await repository.GetStaffAsync();
            return Results.Ok(staff);
        });

        // Get staff by Id
        staffEndpoint.MapGet("/{id}", async (int id, StaffRepository repository) =>
        {
            var staff = await repository.GetStaffByIdAsync(id);
            return staff is not null ? Results.Ok(staff) : Results.NotFound();
        });

        // Create a new staff
        staffEndpoint.MapPost("/", async (Staff staff, StaffRepository repository) =>
        {
            await repository.CreateStaffAsync(staff);
            return Results.Created($"/{staff.Id}", staff);
        });

        // Update an existing staff
        staffEndpoint.MapPut("/{id}", async (int id, Staff staff, StaffRepository repository) =>
        {
            var existingStaff = await repository.GetStaffByIdAsync(id);
            if (existingStaff is null)
            {
                return Results.NotFound();
            }

            await repository.UpdateStaffAsync(id, staff);
            return Results.NoContent();
        });

        // Delete a staff
        staffEndpoint.MapDelete("/{id}", async (int id, StaffRepository repository) =>
        {
            var staff = await repository.GetStaffByIdAsync(id);
            if (staff is null)
            {
                return Results.NotFound();
            }

            await repository.DeleteStaffAsync(id);
            return Results.NoContent();
        });
    }
}
