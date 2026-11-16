using Microsoft.EntityFrameworkCore;
using MopjesShopjes.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Connect to DB
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add health check for DB
    builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
}

WebApplication app = builder.Build();

// Handle DB migration on app startup automatically
if (!builder.Environment.IsEnvironment("Testing"))
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}

// Execute health check
if (!builder.Environment.IsEnvironment("Testing"))
{
    app.MapHealthChecks("/health");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapItemEndpoints();

app.Run();

public partial class Program { }
