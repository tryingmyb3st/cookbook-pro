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

    public async Task<long> AddIngredientAsync(CookbookCommon.DTO.IngredientCreate ingredientCreate)
    {
        var ingredient = new Ingredient
        {
            Name = ingredientCreate.Name,
            Protein = ingredientCreate.Protein,
            Fats = ingredientCreate.Fats,
            Carbs = ingredientCreate.Carbs,
            Calories = ingredientCreate.Calories,
        };

        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();

        return ingredient.Id;
    }
}