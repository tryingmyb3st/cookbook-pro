using CookbookCommon.DTO;

namespace CookbookTheMealDB;

public interface IMealDBService
{
    Task<Recipe?> GetRandomRecipeAsync();
}