using System.ComponentModel.DataAnnotations;

namespace CookbookCommon.DTO;

public class IngredientCreate
{
    [Required(ErrorMessage = "Имя не может быть не задано")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле \"Белки\" не может быть не задано")]
    public decimal Protein { get; set; }

    [Required(ErrorMessage = "Поле \"Жиры\" не может быть не задано")]
    public decimal Fats { get; set; }

    [Required(ErrorMessage = "Поле \"Углеводы\" не может быть не задано")]
    public decimal Carbs { get; set; }

    [Required(ErrorMessage = "Поле \"Калории\" не может быть не задано")]
    public decimal Calories { get; set; }
}