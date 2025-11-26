namespace CookbookCommon.DTO;

public class Recipe: RecipeBase
{
    public List<Ingredient> Ingredients { get; set; } = [];
}