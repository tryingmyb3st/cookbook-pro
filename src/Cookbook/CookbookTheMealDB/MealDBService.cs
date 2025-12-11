using CookbookCommon.DTO;
using CookbookDB.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CookbookTheMealDB;

public class MealDBService: IMealDBService
{
    private readonly HttpClient _httpClient;
    private readonly IngredientRepository _ingredientRepository;

    private readonly ILogger<MealDBService> _logger;

    public MealDBService(HttpClient httpClient, IngredientRepository ingredientRepository, ILogger<MealDBService> logger)
    {
        _httpClient = httpClient;
        _ingredientRepository = ingredientRepository;

        _logger = logger;

        _httpClient.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<Recipe?> GetRandomRecipeAsync()
    {

        _logger.LogInformation("Запрос случайного рецепта из TheMealDB API");

        var response = await _httpClient.GetAsync("random.php");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var mealDbResponse = JsonSerializer.Deserialize<MealResponse>(json, options);

        if (mealDbResponse?.Meals == null || mealDbResponse.Meals.Count() == 0)
        {
            _logger.LogWarning("Рецепт не найден в ответе API");
            return null;
        }

        var recipe = await ParseToRecipe(mealDbResponse.Meals[0]);

        return recipe;

    }

    private async Task<Recipe> ParseToRecipe(Meal meal)
    {
        var recipe = new Recipe
        {
            Id = int.TryParse(meal.IdMeal, out int id) ? id : 0,
            Name = meal.StrMeal ?? string.Empty,
            Instruction = meal.StrInstructions ?? string.Empty,
            FileName = meal.StrMealThumb,
        };

        var ingredients = new List<string>();
        var measures = new List<string>();

        for (int i = 1; i <= 20; i++)
        {
            var ingredientProperty = typeof(Meal).GetProperty($"StrIngredient{i}");
            var measureProperty = typeof(Meal).GetProperty($"StrMeasure{i}");

            var ingredient = ingredientProperty?.GetValue(meal) as string;
            var measure = measureProperty?.GetValue(meal) as string;

            if (string.IsNullOrWhiteSpace(ingredient))
            {
                break;
            }
            ingredients.Add(ingredient);
            measures.Add(measure ?? string.Empty);
        }

        var exictingIngredients = await _ingredientRepository.AddAndGetMany(ingredients);

        for (int i = 1; i < ingredients.Count; i++)
        {
            var ingredientName = ingredients[i];
            var parsedMeasure = MeasureParser.ParseMeasure(measures[i]?.Trim());
            var exictingIngredient = exictingIngredients.First(i => i.Name.Contains(ingredientName));

            var ingredient = new Ingredient
            {
                Id = exictingIngredient.Id,
                Name = ingredientName,
                Protein = exictingIngredient?.Protein,
                Fats = exictingIngredient?.Fats,
                Carbs = exictingIngredient?.Carbs,
                Calories = exictingIngredient?.Calories,
                Weight = parsedMeasure.Quantity,
            };

            recipe.Ingredients.Add(ingredient);
        }

        return recipe;
    }
}