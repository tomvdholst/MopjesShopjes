public class ItemStore
{
    private readonly List<Item> _items = new()
    {
        new Item(1, "Milk", false),
        new Item(2, "Bread", false),
        new Item(3, "Eggs", true),
    };

    public IReadOnlyList<Item> GetAll() => _items;
}