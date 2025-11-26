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

    public async Task<Ingredient?> Get(string name)
    {
        return await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == name);
    }

    public async Task<long> AddIngredientAsync(CookbookCommon.DTO.IngredientBase ingredientDto)
    {
        var ingredient = new Ingredient
        {
            Name = ingredientDto.Name,
            Protein = ingredientDto.Protein,
            Fats = ingredientDto.Fats,
            Carbs = ingredientDto.Carbs,
            Calories = ingredientDto.Calories,
        };

        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();

        return ingredient.Id;
    }
}