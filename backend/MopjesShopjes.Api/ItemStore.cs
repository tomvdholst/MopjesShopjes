public class ItemStore
{
    private readonly List<Item> _items = new()
    {
        new Item{Identifier = 1, Name = "Milk", IsChecked = false},
        new Item{Identifier = 2, Name = "Bread", IsChecked = false},
        new Item{Identifier = 3, Name = "Eggs", IsChecked = true},
    };

    private int _nextId = 4;

    public IReadOnlyList<Item> GetAll() => _items;

    public Item Add(string name)
    {
        var item = new Item { Identifier = _nextId++, Name = name, IsChecked = false };
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
        var item = _items.FirstOrDefault(i => i.Identifier == id);
        if (item is null)
        {
            return null;
        }

        item.IsChecked = !item.IsChecked;
        return item;
    }
}