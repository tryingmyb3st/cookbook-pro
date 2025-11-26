namespace CookbookDB.Models;

/// <summary>
/// ингредиенты, использованные в рецептах
/// </summary>
public partial class RecipeIngredient
{
    public long RecipeId { get; set; }

    public long IngredientId { get; set; }

    public decimal? Weight { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
