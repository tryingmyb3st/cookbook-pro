using System.ComponentModel.DataAnnotations;

namespace CookbookCommon.DTO;

public class RecipeCreate
{
    [Required(ErrorMessage = "Имя не может быть не задано")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Количество порций не может быть не задано")]
    public int ServingsNumber { get; set; }

    [Required(ErrorMessage = "Инструкция не может быть пустой")]
    public string Instruction { get; set; } = string.Empty;

    [Required(ErrorMessage = "Список ингредиентов не может быть пустым")]
    [MinLength(1, ErrorMessage = "Список ингредиентов не может быть пустым")]
    public IEnumerable<RecipeIngredient> Ingredients { get; set; } = [];

    public string? FileName { get; set; } = string.Empty;
}