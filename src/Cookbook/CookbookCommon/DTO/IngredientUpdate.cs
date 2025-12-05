using System.ComponentModel.DataAnnotations;

namespace CookbookCommon.DTO;

public class IngredientUpdate : IngredientCreate
{
    [Required(ErrorMessage = "Идентификатор не может быть не задан")]
    public long Id { get; set; }
}