using AutoMapper;
using CookbookCommon.DTO;
using CookbookDB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly RecipeRepository _recipeRepository;

    private readonly ILogger<RecipeController> _logger;
    private readonly IMapper _mapper;
    public RecipeController(RecipeRepository recipeRepository, ILogger<RecipeController> logger, IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{recipeId:int}")]
    public async Task<Recipe> Get(int recipeId)
    {
        var recipe = await _recipeRepository.Get(recipeId);
        return _mapper.Map<Recipe>(recipe);
    }

    [HttpPost("search")]
    public async Task<Recipe> Search(string recipeName)
    {
        var recipe = await _recipeRepository.Get(recipeName);
        return _mapper.Map<Recipe>(recipe);
    }

    [HttpPost]
    public async Task<long> Create(RecipeCreate recipe)
    {
        var id = await _recipeRepository.AddRecipeAsync(recipe);
        return id;
    }
}
