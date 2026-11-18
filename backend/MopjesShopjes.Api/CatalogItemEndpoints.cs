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
            if (request.DefaultQuantity <= 0)
            {
                return Results.BadRequest("Default quantity should be greater than zero.");
            }
            if (request.StepSize <= 0)
            {
                return Results.BadRequest("Step size should be greater than zero.");
            }
            if (!Enum.IsDefined(request.Unit))
            {
                return Results.BadRequest("Unit is not valid.");
            }
            CatalogItem item = new CatalogItem {Name = name, DefaultQuantity = request.DefaultQuantity, StepSize = request.StepSize, Unit = request.Unit };
            db.CatalogItems.Add(item);
            await db.SaveChangesAsync();
            return Results.CreatedAtRoute("GetCatalogItemById", new { id = item.Id}, item);
        });
    }
}
