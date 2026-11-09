namespace MopjesShopjes.Api;

public class CatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public QuantityUnit Unit { get; set; }
    public decimal DefaultQuantity { get; set; }
    public decimal StepSize { get; set; }
}
