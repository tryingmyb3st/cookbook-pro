using AutoMapper;
using CookbookCommon.DTO;
using CookbookDB.Repositories;
using CookbookTheMealDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]/[action]")]
[Authorize]
public class RecipeController(RecipeRepository recipeRepository, IMealDBService mealDBService, IMapper mapper) : ControllerBase
{
    private readonly RecipeRepository _recipeRepository = recipeRepository;
    private readonly IMealDBService _mealDBService = mealDBService;

    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [AllowAnonymous]
    public async Task<Recipe> Get([FromQuery] int id)
    {
        var recipe = await _recipeRepository.Get(id);
        return _mapper.Map<Recipe>(recipe);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<Recipe[]> Search([FromQuery] string name)
    {
        var recipes = await _recipeRepository.Search(name);
        return recipes.Select(_mapper.Map<Recipe>).ToArray();
    }

    [HttpPost]
    public async Task<long> Create(RecipeCreate recipe)
    {
        var id = await _recipeRepository.AddRecipeAsync(recipe, GetCurrentUserId());
        return id;
    }

    [HttpPost]
    public async Task Update(RecipeUpdate recipe)
    {
        await _recipeRepository.UpdateRecipeAsync(recipe, GetCurrentUserId());
    }

    [HttpDelete]
    public async Task Delete([FromQuery] long id)
    {
        await _recipeRepository.DeleteRecipeAsync(id, GetCurrentUserId());
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<Recipe?> GetRandomFromTheMealDB()
    {
        return await _mealDBService.GetRandomRecipeAsync();
    }

    private long GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value
            ?? User.FindFirst("nameid")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
        {
            throw new UnauthorizedAccessException("Не удалось определить пользователя");
        }

        return userId;
    }
}