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
            entity.Property(c => c.Name).HasMaxLength(CatalogItem.NameMaxLength);
            entity.Property(c => c.Unit).HasConversion<string>();
            entity.Property(c => c.DefaultQuantity).HasPrecision(8, 3);
            entity.Property(c => c.StepSize).HasPrecision(8, 3);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            // Define relation between Item and CatalogItem
            entity.HasOne(i => i.CatalogItem)          // Item has one catalog item
                  .WithMany()                          // A catalog item can belong to many items
                  .HasForeignKey(i => i.CatalogItemId) // Linked through the catalog item's ID
                  .OnDelete(DeleteBehavior.Restrict);  // Deleting a CatalogItem still in use is refused

            entity.Property(i => i.Name).HasMaxLength(Item.NameMaxLength);
            entity.Property(i => i.QuantityText).HasMaxLength(Item.QuantityTextMaxLength);
            entity.Property(i => i.Quantity).HasPrecision(8, 3);

            // Create check constraint: item must have either Name or CatalogItemId
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Items_NameOrCatalogItem",
                "(\"Name\" IS NULL) <> (\"CatalogItemId\" IS NULL)"));
        });
    }
}
