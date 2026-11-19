namespace MopjesShopjes.Api;

public static class QuantityLimits
{
    public const int Precision = 8;
    public const int Scale = 3;
    public const decimal Max = 99999.999m; // Maximum value for precision 8 and scale 3
    public static string? ValidateQuantity(decimal value, string fieldName)
    {
        // Verify that input value is greater than zero 
        if (value <= 0)
        {
            return $"{fieldName} must be greater than zero.";
        }
        // Check if input value is too large
        if (value > Max)
        {
            return $"{fieldName} has a maximum value of {Max}.";
        }
        // Check if the scale of the input value is too high
        if (decimal.Round(value, Scale) != value)
        {
            return $"{fieldName} can have at most {Scale} decimals.";
        }
        return null;
    }
}