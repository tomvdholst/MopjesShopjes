namespace MopjesShopjes.Api;

public record CreateCatalogItemRequest(string Name, QuantityUnit Unit, decimal DefaultQuantity, decimal StepSize);
