using CookbookDB.Models;
using Microsoft.EntityFrameworkCore;
using Recipe = CookbookDB.Models.Recipe;

namespace CookbookDB.Repositories;
public class RecipeRepository(CookbookDbContext context)
{
    private readonly CookbookDbContext _context = context;

    public async Task<Recipe?> Get(int id)
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recipe?> Get(string name)
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<long> AddRecipeAsync(CookbookCommon.DTO.RecipeCreate recipeCreate)
    {
        var ingredientIds = recipeCreate.Ingredients.Select(i => i.IngredientId);
        var existingIngredients = _context.Ingredients.Where(i => ingredientIds.Contains(i.Id));

        var notFoundIngredients = ingredientIds.Except(existingIngredients.Select(i => i.Id));

        if (notFoundIngredients.Any())
        {
            throw new Exception("Не найден(ы) ингредиент(ы) по идентификатору: " +
                string.Join(", ", notFoundIngredients.Select(i => i.ToString())));
        }

        var duplicates = ingredientIds.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);

        if (duplicates.Any())
        {
            throw new Exception("Дублирование ингредиента(ов) по идентификатору: " +
               string.Join(", ", duplicates.Select(i => i.ToString())));
        }

        var recipe = new Recipe
        {
            Name = recipeCreate.Name,
            Instruction = recipeCreate.Instruction,
            ServingsNumber = recipeCreate.ServingsNumber,
        };

        await _context.Recipes.AddAsync(recipe);

        var id = recipe.Id;
        recipe.RecipeIngredients = recipeCreate.Ingredients
            .Select(i => new RecipeIngredient
            {
                RecipeId = id,
                IngredientId = i.IngredientId,
                Weight = i.Weight,
            }).ToArray();

        await _context.SaveChangesAsync();

        return recipe.Id;
    }
}