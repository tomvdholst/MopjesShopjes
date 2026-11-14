namespace MopjesShopjes.Api;

public class Item
{
    public const int NameMaxLength = 100;
    public const int QuantityTextMaxLength = 100;
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsChecked { get; set; }
    public double SortOrder { get; set; }
    public int? CatalogItemId { get; set; }
    public CatalogItem? CatalogItem { get; set; }
    public decimal? Quantity { get; set; }
    public string? QuantityText { get; set; }
}
