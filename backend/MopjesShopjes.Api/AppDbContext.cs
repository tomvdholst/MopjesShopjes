using Microsoft.EntityFrameworkCore;

namespace MopjesShopjes.Api;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogItem>(entity =>
        {
            // Add Name as a DB Index, making it unique and faster to search
            entity.HasIndex(c => c.Name).IsUnique();

            // Define rules for properties
            entity.Property(c => c.Name).HasMaxLength(100);
            
            // Store as text, so reordering the enum can't change the meaning of existing rows
            entity.Property(c => c.Unit).HasConversion<string>();

            entity.Property(c => c.DefaultQuantity).HasPrecision(8, 3);
            
            entity.Property(c => c.StepSize).HasPrecision(8, 3);
        });
    }
}