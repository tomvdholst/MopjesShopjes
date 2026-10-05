namespace MopjesShopjes.Api.Tests;

public class ItemTests
{
    [Fact]
    public void NewItem_StoresAssignedValues()
    {
        // Arrange
        Item item = new Item { Id = 1, Name = "Milk", IsChecked = false };

        // Assert
        Assert.Equal(1, item.Id);
        Assert.Equal("Milk", item.Name);
        Assert.False(item.IsChecked);
    }
}