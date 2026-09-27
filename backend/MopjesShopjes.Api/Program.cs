var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ItemStore>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.MapGet("/api/items", (ItemStore store) => store.GetAll());
app.MapPost("/api/items", (ItemStore store, Item newItem) => 
{
    var created = store.Add(newItem.Name);
    return Results.Created($"/api/items/{created.Identifier}", created);
});
app.MapDelete("/api/items/{id}", (int id, ItemStore store) =>
{
    var deleted = store.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});
app.MapPatch("/api/items/{id}", (int id, ItemStore store) =>
{
    var updated = store.ToggleChecked(id);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.Run();