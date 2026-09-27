public class ItemStore
{
    private readonly List<Item> _items = new()
    {
        new Item(1, "Milk", false),
        new Item(2, "Bread", false),
        new Item(3, "Eggs", true),
    };

    private int _nextId = 4;

    public IReadOnlyList<Item> GetAll() => _items;

    public Item Add(string name)
    {
        var item = new Item(_nextId++, name, false);
        _items.Add(item);
        return item;
    }

    public bool Delete(int id)
    {
        var item = _items.FirstOrDefault(i => i.Identifier == id);
        if (item is null)
        {
            return false;
        }
        _items.Remove(item);
        return true;
    }

    public Item? ToggleChecked(int id)
    {
        var index = _items.FindIndex(i => i.Identifier == id);
        if (index == -1)
        {
            return null;
        }

        var updated = _items[index] with { IsChecked = !_items[index].IsChecked };
        _items[index] = updated;
        return updated;
    }
}