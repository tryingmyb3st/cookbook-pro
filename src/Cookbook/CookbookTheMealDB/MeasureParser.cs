namespace CookbookTheMealDB;

public static class MeasureParser
{
    public class ParsedMeasure
    {
        public decimal? Quantity { get; set; }
        public string Unit { get; set; } = "гр";
    }

    public static ParsedMeasure ParseMeasure(string? measure)
    {
        var result = new ParsedMeasure();

        if (string.IsNullOrWhiteSpace(measure) ||
            measure.Trim() == string.Empty)
        {
            return result;
        }

        var trimmedMeasure = measure.Trim();

        if (trimmedMeasure.ToLower() == "to taste" ||
            trimmedMeasure.ToLower() == "as needed" ||
            trimmedMeasure.ToLower() == "pinch" ||
            trimmedMeasure.ToLower() == "dash")
        {
            result.Quantity = null;
            result.Unit = "по вкусу";
            return result;
        }

        try
        {
            var numberMatch = System.Text.RegularExpressions.Regex.Match(
                trimmedMeasure,
                @"^(\d+(\.\d+)?)|^(\d+\s*/\s*\d+)"
            );

            if (numberMatch.Success)
            {
                if (numberMatch.Value.Contains('/'))
                {
                    var fractionParts = numberMatch.Value.Split('/');
                    if (fractionParts.Length == 2 &&
                        decimal.TryParse(fractionParts[0], out decimal numerator) &&
                        decimal.TryParse(fractionParts[1], out decimal denominator) &&
                        denominator != 0)
                    {
                        result.Quantity = numerator / denominator;
                    }
                }
                else
                {
                    if (decimal.TryParse(numberMatch.Value, out decimal quantity))
                    {
                        result.Quantity = quantity;
                    }
                }

                trimmedMeasure = trimmedMeasure.Substring(numberMatch.Length).Trim();
            }

            if (!string.IsNullOrWhiteSpace(trimmedMeasure))
            {
                var unitMatch = System.Text.RegularExpressions.Regex.Match(
                    trimmedMeasure,
                    @"^([a-zA-Zа-яА-Я]+)"
                );

                if (unitMatch.Success)
                {
                    var unit = unitMatch.Value.ToLower();
                    result.Unit = NormalizeUnit(unit);
                }
                else
                {
                    var abbreviationMatch = System.Text.RegularExpressions.Regex.Match(
                        trimmedMeasure,
                        @"^([a-zA-Z]{1,3})"
                    );

                    if (abbreviationMatch.Success)
                    {
                        result.Unit = NormalizeUnit(abbreviationMatch.Value);
                    }
                }
            }
        }
        catch
        {
            result.Quantity = null;
            result.Unit = "гр";
        }

        return result;
    }

    private static string NormalizeUnit(string unit)
    {
        return unit.ToLower() switch
        {
            "g" or "gram" or "grams" => "гр",
            "kg" or "kilo" or "kilos" or "kilogram" or "kilograms" => "кг",
            "mg" or "milligram" or "milligrams" => "мг",
            "ml" or "milliliter" or "milliliters" => "мл",
            "l" or "liter" or "liters" or "litre" or "litres" => "л",
            "cup" or "cups" => "чашка",
            "tablespoon" or "tablespoons" or "tbsp" => "ст.л",
            "teaspoon" or "teaspoons" or "tsp" => "ч.л",
            "ounce" or "ounces" or "oz" => "унция",
            "pound" or "pounds" or "lb" => "фунт",
            "pinch" or "pinches" => "щепотка",
            "slice" or "slices" => "ломтик",
            "piece" or "pieces" => "кусок",
            "clove" or "cloves" => "зубчик",
            "bunch" or "bunches" => "пучок",
            "can" or "cans" => "банка",
            "jar" or "jars" => "банка",
            "packet" or "packets" => "пакет",
            "bag" or "bags" => "пакет",
            "bottle" or "bottles" => "бутылка",
            "box" or "boxes" => "коробка",
            "package" or "packages" => "упаковка",

            "грамм" or "граммов" => "гр",
            "килограмм" or "килограммов" => "кг",
            "миллиграмм" or "миллиграммов" => "мг",
            "миллилитр" or "миллилитров" => "мл",
            "литр" or "литров" => "л",
            "стакан" or "стаканов" => "стакан",
            "ложка" or "ложек" => "ст.л",
            "ложечка" or "ложечек" => "ч.л",
            "столовая" => "ст.л",
            "чайная" => "ч.л",
            "штука" or "штук" => "шт",
            "пучок" or "пучков" => "пучок",
            "банка" or "банок" => "банка",
            "пакет" or "пакетов" => "пакет",
            "бутылка" or "бутылок" => "бутылка",
            "коробка" or "коробок" => "коробка",
            "упаковка" or "упаковок" => "упаковка",

            _ => unit.ToLower()
        };
    }
}