namespace CookbookCommon.DTO;

public class RecipeBase
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? Weight { get; set; }

    public int? ServingsNumber { get; set; }

    public string Instruction { get; set; } = string.Empty;

    public string? FileName { get; set; } = string.Empty;

    public long UserId { get; set; }

    public string UserName { get; set; } = string.Empty;
}