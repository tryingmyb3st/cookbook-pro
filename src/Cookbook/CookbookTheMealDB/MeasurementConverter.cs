namespace CookbookTheMealDB;

public static class MeasurementConverter
{
    private static readonly Dictionary<string, decimal> _measurementConversions = new()
        {
            { "cup", 240m },
            { "cups", 240m },
            { "tablespoon", 15m },
            { "tablespoons", 15m },
            { "tbsp", 15m },
            { "teaspoon", 5m },
            { "teaspoons", 5m },
            { "tsp", 5m },
            { "ounce", 28.35m },
            { "ounces", 28.35m },
            { "oz", 28.35m },
            { "pound", 453.59m },
            { "pounds", 453.59m },
            { "lb", 453.59m },
            { "pint", 473.18m },
            { "pints", 473.18m },
            { "quart", 946.35m },
            { "quarts", 946.35m },
            { "gallon", 3785.41m },
            { "gallons", 3785.41m },
            { "liter", 1000m },
            { "liters", 1000m },
            { "ml", 1m },
            { "milliliter", 1m },
            
            { "clove", 5m },
            { "cloves", 5m },
            { "slice", 30m },
            { "slices", 30m },
            { "piece", 100m },
            { "pieces", 100m },
            { "whole", 200m },
            { "large", 150m },
            { "medium", 100m },
            { "small", 50m },          
        };

    public static decimal? ParseMeasureToGrams(string? measure)
    {
        if (string.IsNullOrWhiteSpace(measure) ||
            measure.ToLower() == "to taste" ||
            measure.ToLower() == "as needed" ||
            measure.ToLower() == "pinch" ||
            measure.Trim() == string.Empty)
        {
            return null;
        }

        try
        {
            decimal quantity = 1;
            string normalizedMeasure = measure.ToLower().Trim();

            var parts = normalizedMeasure.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && decimal.TryParse(parts[0], out decimal parsedQuantity))
            {
                quantity = parsedQuantity;
                normalizedMeasure = string.Join(" ", parts.Skip(1));
            }
            else
            {
                var match = System.Text.RegularExpressions.Regex.Match(measure, @"(\d+)\s*/\s*(\d+)");
                if (match.Success)
                {
                    decimal numerator = decimal.Parse(match.Groups[1].Value);
                    decimal denominator = decimal.Parse(match.Groups[2].Value);
                    quantity = numerator / denominator;
                    normalizedMeasure = normalizedMeasure.Replace(match.Value, "").Trim();
                }
            }

            if (normalizedMeasure.Contains("g") && !normalizedMeasure.Contains("gal"))
            {
                var gramMatch = System.Text.RegularExpressions.Regex.Match(measure, @"(\d+(\.\d+)?)\s*g");
                if (gramMatch.Success && decimal.TryParse(gramMatch.Groups[1].Value, out decimal grams))
                {
                    return grams;
                }
            }

            if (normalizedMeasure.Contains("kg") || normalizedMeasure.Contains("kilo"))
            {
                var kgMatch = System.Text.RegularExpressions.Regex.Match(measure, @"(\d+(\.\d+)?)\s*kg");
                if (kgMatch.Success && decimal.TryParse(kgMatch.Groups[1].Value, out decimal kg))
                {
                    return kg * 1000;
                }
            }

            foreach (var conversion in _measurementConversions)
            {
                if (!string.IsNullOrEmpty(normalizedMeasure) &&
                    normalizedMeasure.Contains(conversion.Key))
                {
                    return quantity * conversion.Value;
                }
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(normalizedMeasure, @"^\d*\s*$") ||
                string.IsNullOrEmpty(normalizedMeasure))
            {
                return quantity * 100;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}