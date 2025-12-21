using Microsoft.EntityFrameworkCore;
using Ingredient = CookbookDB.Models.Ingredient;

namespace CookbookDB.Repositories;

public class IngredientRepository(CookbookDbContext context)
{
    private readonly CookbookDbContext _context = context;

    public async Task<Ingredient?> Get(int id)
    {
        return await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Ingredient[]?> Search(string name)
    {
        return await _context.Ingredients
            .Where(i => i.Name.Contains(name))
            .ToArrayAsync();
    }

    public async Task<List<Ingredient>> AddAndGetMany(List<string> ingredientNames)
    {
        if (ingredientNames == null || ingredientNames.Count == 0)
            return [];

        var ingredients = await _context.Ingredients
            .Where(i => ingredientNames.Contains(i.Name))
            .ToListAsync();

        foreach (var newIngredient in ingredientNames.Except(ingredients.Select(i=> i.Name)))
        {
            var ingredient = new Ingredient
            {
                Name = newIngredient,
                Protein = 0,
                Fats = 0,
                Carbs = 0,
                Calories = 0,
                UserId = 0,
            };
            await _context.Ingredients.AddAsync(ingredient);
            ingredients.Add(ingredient);
        }

        await _context.SaveChangesAsync();
        return ingredients;
    }

    public async Task<long> AddIngredientAsync(CookbookCommon.DTO.IngredientCreate ingredientCreate, long userId)
    {
        var ingredient = new Ingredient
        {
            Name = ingredientCreate.Name,
            Protein = ingredientCreate.Protein,
            Fats = ingredientCreate.Fats,
            Carbs = ingredientCreate.Carbs,
            Calories = ingredientCreate.Calories,
            UserId = userId,
        };

        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();

        return ingredient.Id;
    }
}