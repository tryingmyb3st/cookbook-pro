namespace CookbookDB.Models;

/// <summary>
/// рецепты
/// </summary>
public partial class Recipe
{
    /// <summary>
    /// идентификатор рецепта
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// название рецепта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// вес всего блюда
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// количество порций
    /// </summary>
    public int? ServingsNumber { get; set; }

    /// <summary>
    /// инструкция приготовления
    /// </summary>
    public string? Instruction { get; set; }

    /// <summary>
    /// имя файла
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}