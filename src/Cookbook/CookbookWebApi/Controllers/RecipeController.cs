using AutoMapper;
using CookbookCommon.DTO;
using CookbookDB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]/[action]")]
public class RecipeController(RecipeRepository recipeRepository, IMapper mapper) : ControllerBase
{
    private readonly RecipeRepository _recipeRepository = recipeRepository;

    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<Recipe> Get([FromQuery] int id)
    {
        var recipe = await _recipeRepository.Get(id);
        return _mapper.Map<Recipe>(recipe);
    }

    [HttpGet]
    public async Task<Recipe[]> Search([FromQuery] string name)
    {
        var recipes = await _recipeRepository.Search(name);
        return recipes.Select(_mapper.Map<Recipe>).ToArray();
    }

    [HttpPost]
    public async Task<long> Create(RecipeCreate recipe)
    {
        var id = await _recipeRepository.AddRecipeAsync(recipe);
        return id;
    }

    [HttpPost]
    public async Task Update(RecipeUpdate recipe)
    {
        await _recipeRepository.UpdateRecipeAsync(recipe);
    }

    [HttpDelete]
    public async Task Delete([FromQuery] long id)
    {
        await _recipeRepository.DeleteRecipeAsync(id);
    }
}
