using Microsoft.EntityFrameworkCore;
using MopjesShopjes.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
    
});
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.MapGet("/api/items", async (AppDbContext db) => await db.Items.OrderBy(i => i.Id).ToListAsync());
app.MapPost("/api/items", async (AppDbContext db, Item newItem) => 
{
    var item = new Item {Name = newItem.Name, IsChecked = false};
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/api/items/{item.Id}", item);
});
app.MapDelete("/api/items/{id}", async (AppDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null)
    {
        return Results.NotFound();
    }
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapPatch("/api/items/{id}", async (AppDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) 
    {
        return Results.NotFound();
    }
    item.IsChecked = !item.IsChecked;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

app.Run();

public partial class Program { }
