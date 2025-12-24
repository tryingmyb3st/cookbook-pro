using System.Net;
using System.Net.Http.Json;
using CookbookCommon.DTO;

namespace CookbookTests.Controllers;

public class RecipeControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public RecipeControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private async Task<RecipeCreate> CreateSampleRecipeAsync(HttpClient client, string name = "Test Recipe")
    {
        var ingredientCreate = new IngredientCreate
        {
            Name = "Tomato",
            Protein = 1,
            Fats = 0,
            Carbs = 4,
            Calories = 20
        };

        var ingredientResponse = await client.PostAsJsonAsync("/cookbook/Ingredient/Create", ingredientCreate);
        ingredientResponse.EnsureSuccessStatusCode();
        var ingredientId = await ingredientResponse.Content.ReadFromJsonAsync<long>();

        return new RecipeCreate
        {
            Name = name,
            Instruction = "Test instruction",
            ServingsNumber = 2,
            Ingredients = new[]
            {
                new RecipeIngredient
                {
                    IngredientId = ingredientId,
                    Weight = 100
                }
            }
        };
    }

    [Fact]
    public async Task Create_Then_Get_ReturnsCreatedRecipe()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("recipe@test.com");
        var recipeCreate = await CreateSampleRecipeAsync(client, "Integration Recipe");

        var createResponse = await client.PostAsJsonAsync("/cookbook/Recipe/Create", recipeCreate);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdId = await createResponse.Content.ReadFromJsonAsync<long>();
        createdId.Should().BeGreaterThan(0);

        var getResponse = await client.GetAsync($"/cookbook/Recipe/Get?id={createdId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var recipe = await getResponse.Content.ReadFromJsonAsync<Recipe>();
        recipe.Should().NotBeNull();
        recipe!.Name.Should().Be("Integration Recipe");
        recipe.ServingsNumber.Should().Be(2);
        recipe.Ingredients.Should().HaveCount(1);
    }

    [Fact]
    public async Task Search_ReturnsMatchingRecipes()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("searchrecipe@test.com");

        var recipe1 = await CreateSampleRecipeAsync(client, "Tomato Soup");
        var recipe2 = await CreateSampleRecipeAsync(client, "Potato Soup");
        var recipe3 = await CreateSampleRecipeAsync(client, "Carrot Salad");

        foreach (var recipeCreate in new[] { recipe1, recipe2, recipe3 })
        {
            var response = await client.PostAsJsonAsync("/cookbook/Recipe/Create", recipeCreate);
            response.EnsureSuccessStatusCode();
        }

        var searchResponse = await client.GetAsync("/cookbook/Recipe/Search?name=Soup");
        searchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var recipes = await searchResponse.Content.ReadFromJsonAsync<Recipe[]>();
        recipes.Should().NotBeNull();
        recipes!.Length.Should().Be(2);
        recipes.Should().Contain(r => r.Name == "Tomato Soup");
        recipes.Should().Contain(r => r.Name == "Potato Soup");
        recipes.Should().NotContain(r => r.Name == "Carrot Salad");
    }

    [Fact]
    public async Task Update_ChangesRecipeData()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("updaterecipe@test.com");

        var recipeCreate = await CreateSampleRecipeAsync(client, "Original Recipe");
        var createResponse = await client.PostAsJsonAsync("/cookbook/Recipe/Create", recipeCreate);
        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content.ReadFromJsonAsync<long>();

        var update = new RecipeUpdate
        {
            Id = id,
            Name = "Updated Recipe",
            Instruction = "Updated instruction",
            ServingsNumber = 4,
            Ingredients = recipeCreate.Ingredients
        };

        var updateResponse = await client.PostAsJsonAsync("/cookbook/Recipe/Update", update);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await client.GetAsync($"/cookbook/Recipe/Get?id={id}");
        var recipe = await getResponse.Content.ReadFromJsonAsync<Recipe>();

        recipe.Should().NotBeNull();
        recipe!.Name.Should().Be("Updated Recipe");
        recipe.Instruction.Should().Be("Updated instruction");
        recipe.ServingsNumber.Should().Be(4);
    }

    [Fact]
    public async Task Delete_RemovesRecipe()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("deleterecipe@test.com");

        var recipeCreate = await CreateSampleRecipeAsync(client, "Recipe To Delete");
        var createResponse = await client.PostAsJsonAsync("/cookbook/Recipe/Create", recipeCreate);
        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content.ReadFromJsonAsync<long>();

        var deleteResponse = await client.DeleteAsync($"/cookbook/Recipe/Delete?id={id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await client.GetAsync($"/cookbook/Recipe/Get?id={id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetRandomFromTheMealDB_ReturnsFakeRecipe()
    {
        var client = await _factory.CreateAuthenticatedClientAsync("randomrecipe@test.com");

        var response = await client.GetAsync("/cookbook/Recipe/GetRandomFromTheMealDB");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var recipe = await response.Content.ReadFromJsonAsync<Recipe>();
        recipe.Should().NotBeNull();
        recipe!.Name.Should().Be("Test MealDB Recipe");
    }
}
