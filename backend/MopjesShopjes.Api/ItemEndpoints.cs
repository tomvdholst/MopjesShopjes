using Microsoft.EntityFrameworkCore;

namespace MopjesShopjes.Api;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/items");

        group.MapGet("/", async (AppDbContext db) => await db.Items.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).ToListAsync());

        group.MapPost("/", async (AppDbContext db, Item newItem) => 
        {
            if (string.IsNullOrWhiteSpace(newItem.Name))
            {
                return Results.BadRequest("Name is required.");
            }
            if (newItem.Name.Length > Item.NameMaxLength)
            {
                return Results.BadRequest($"Name's maximum characters is {Item.NameMaxLength}.");
            }
            Item item = new Item {Name = newItem.Name, IsChecked = false, SortOrder = db.Items.Any() ? db.Items.Max(i => i.SortOrder) + 1 : 1};
            db.Items.Add(item);
            await db.SaveChangesAsync();
            return Results.Created($"/api/items/{item.Id}", item);
        });

        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            Item? item = await db.Items.FindAsync(id);
            if (item is null)
            {
                return Results.NotFound();
            }
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapPatch("/{id}", async (AppDbContext db, int id) =>
        {
            Item? item = await db.Items.FindAsync(id);
            if (item is null) 
            {
                return Results.NotFound();
            }
            item.IsChecked = !item.IsChecked;
            await db.SaveChangesAsync();
            return Results.Ok(item);
        });

        group.MapPatch("/{id}/order", async (AppDbContext db, int id, UpdateItemOrderRequest request) =>
        {
            Item? item = await db.Items.FindAsync(id);
            if (item is null) 
            {
                return Results.NotFound();
            }
            item.SortOrder = request.SortOrder;
            await db.SaveChangesAsync();
            return Results.Ok(item);
        });

    }
}
