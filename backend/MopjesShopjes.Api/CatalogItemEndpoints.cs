using Microsoft.EntityFrameworkCore;

namespace MopjesShopjes.Api;

public static class CatalogItemEndpoints
{
    public static void MapCatalogItemEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/catalog-items");

        group.MapGet("/", async (AppDbContext db) => await db.CatalogItems.OrderBy(c => c.Name).ToListAsync());

        group.MapGet("/{id:int}", async (AppDbContext db, int id) =>
        {
            CatalogItem? item = await db.CatalogItems.FindAsync(id);
            if (item is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(item);
        }).WithName("GetCatalogItemById");

        group.MapPost("/", async (AppDbContext db, CreateCatalogItemRequest request) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Name is required.");
            }
            string name = request.Name.Trim();
            if (name.Length > CatalogItem.NameMaxLength)
            {
                return Results.BadRequest($"Name's maximum characters is {CatalogItem.NameMaxLength}.");
            }
            string? error = QuantityLimits.ValidateQuantity(request.DefaultQuantity, "Default quantity");
            if (error is not null)
            {
                return Results.BadRequest(error);
            }
            error = QuantityLimits.ValidateQuantity(request.StepSize, "Step size");
            if (error is not null)
            {
                return Results.BadRequest(error);
            }
            if (!Enum.IsDefined(request.Unit))
            {
                return Results.BadRequest("Unit is not valid.");
            }
            bool nameExists = await db.CatalogItems.AnyAsync(c => c.Name.ToLower() == name.ToLower());
            if (nameExists)
            {
                return Results.Conflict($"A catalog item named '{name}' already exists.");
            }
            CatalogItem item = new CatalogItem {Name = name, DefaultQuantity = request.DefaultQuantity, StepSize = request.StepSize, Unit = request.Unit };
            db.CatalogItems.Add(item);
            await db.SaveChangesAsync();
            return Results.CreatedAtRoute("GetCatalogItemById", new { id = item.Id}, item);
        });
    }
}
