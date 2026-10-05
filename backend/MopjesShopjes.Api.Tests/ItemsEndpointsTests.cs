using System.Net.Http.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace MopjesShopjes.Api.Tests;

public class ItemsEndpointsTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ItemsEndpointsTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    // Add item to test DB without using http
    private async Task<Item> SeedItemAsync(string name, bool isChecked = false)
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Item item = new Item { Name = name, IsChecked = isChecked };
        db.Items.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    [Fact]
    public async Task GetItems_ReturnsEmptyList_WhenNoItemsExist()
    {
        // Retrieve items
        HttpResponseMessage response = await _client.GetAsync("/api/items");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        List<Item>? items = await response.Content.ReadFromJsonAsync<List<Item>>();
        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Fact]
    public async Task PostItem_CreatesAndPersistsItem()
    {
        // Post an item "Milk"
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/items", new { name = "Milk" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        // Verify the returned result is valid
        Item? createdItem = await response.Content.ReadFromJsonAsync<Item>();
        Assert.NotNull(createdItem);
        Assert.Equal("Milk", createdItem.Name);
        Assert.True(createdItem.Id > 0);
        Assert.False(createdItem.IsChecked);
        Assert.Equal($"/api/items/{createdItem.Id}", response.Headers.Location?.ToString());

        // Verify the new item is in the database
        HttpResponseMessage getResponse = await _client.GetAsync("/api/items");
        List<Item>? getItems = await getResponse.Content.ReadFromJsonAsync<List<Item>>();
        Assert.Contains(getItems!, i => i.Id == createdItem.Id && i.Name == "Milk");
    }

    [Fact]
    public async Task DeleteItem_RemovesItem_WhenItemExists()
    {
        // Add item to DB
        Item seededItem = await SeedItemAsync("Milk", true);

        // Delete the item
        HttpResponseMessage response = await _client.DeleteAsync($"/api/items/{seededItem.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Retrieve the item list again and check if the item was removed
        HttpResponseMessage getResponse = await _client.GetAsync("/api/items");
        List<Item>? items = await getResponse.Content.ReadFromJsonAsync<List<Item>>();
        Assert.DoesNotContain(items!, i => i.Id == seededItem.Id);
    }

    [Fact]
    public async Task DeleteItem_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Attempt to delete non-existing item
        HttpResponseMessage response = await _client.DeleteAsync("/api/items/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchItem_ToggleIsChecked_WhenItemExists()
    {
        // Add an item to the test DB
        Item seededItem = await SeedItemAsync("Milk", false);

        // Toggle the item
        HttpResponseMessage response = await _client.PatchAsync($"/api/items/{seededItem.Id}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify the response body item is toggled
        Item? toggledItem = await response.Content.ReadFromJsonAsync<Item>();
        Assert.NotNull(toggledItem);
        Assert.True(toggledItem?.IsChecked);

        // Retrieve items from the DB
        HttpResponseMessage getResponse = await _client.GetAsync("/api/items");
        List<Item>? getItems = await getResponse.Content.ReadFromJsonAsync<List<Item>>();
        Assert.NotNull(getItems);

        // Verify the item in the DB is toggled
        Item? toggledDbItem = getItems!.FirstOrDefault<Item>(i => i.Id == seededItem.Id);
        Assert.NotNull(toggledDbItem);
        Assert.True(toggledDbItem.IsChecked);
    }

    [Fact]
    public async Task PatchItem_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Attempt to toggle non-existing item
        HttpResponseMessage response = await _client.PatchAsync("/api/items/9999", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
