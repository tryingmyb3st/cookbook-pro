namespace CookbookDB.Models;

/// <summary>
/// ингредиенты
/// </summary>
public partial class Ingredient
{
    /// <summary>
    /// идентификатор ингредиента
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// название ингредиента
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// белки
    /// </summary>
    public decimal? Protein { get; set; }

    /// <summary>
    /// жиры
    /// </summary>
    public decimal? Fats { get; set; }

    /// <summary>
    /// углеводы
    /// </summary>
    public decimal? Carbs { get; set; }

    /// <summary>
    /// ккал
    /// </summary>
    public decimal? Calories { get; set; }

    /// <summary>
    /// идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
