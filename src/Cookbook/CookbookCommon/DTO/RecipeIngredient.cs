using System.ComponentModel.DataAnnotations;

namespace CookbookCommon.DTO;

public class RecipeIngredient
{
    [Required(ErrorMessage = "Идентификатор не может быть не задан")]
    public long? IngredientId { get; set; }

    [Required(ErrorMessage = "Вес не может быть не задан")]
    public decimal? Weight { get; set; }
}
