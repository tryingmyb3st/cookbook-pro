namespace CookbookCommon.DTO;

public class IngredientBase
{
    public long? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? Protein { get; set; }

    public decimal? Fats { get; set; }

    public decimal? Carbs { get; set; }

    public decimal? Calories { get; set; }
}