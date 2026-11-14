namespace MopjesShopjes.Api;

public class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsChecked { get; set; }
    public double SortOrder { get; set; }
    public int? CatalogItemId { get; set; }
    public CatalogItem? CatalogItem { get; set; }
    public decimal? Quantity { get; set; }
    public string? QuantityText { get; set; }
}
