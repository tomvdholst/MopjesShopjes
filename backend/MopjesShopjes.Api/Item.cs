namespace MopjesShopjes.Api;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
    public double SortOrder { get; set; }
}
