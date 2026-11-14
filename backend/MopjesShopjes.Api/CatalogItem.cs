namespace MopjesShopjes.Api;

public class CatalogItem
{
    public const int NameMaxLength = Item.NameMaxLength; // CatalogItem name max length should be the same as Item max length
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public QuantityUnit Unit { get; set; }
    public decimal DefaultQuantity { get; set; }
    public decimal StepSize { get; set; }
}
