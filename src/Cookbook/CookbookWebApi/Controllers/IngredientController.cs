using AutoMapper;
using CookbookCommon.DTO;
using CookbookDB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]")]
public class IngredientController: ControllerBase
{
    private readonly IngredientRepository _ingredientRepository;

    private readonly ILogger<IngredientController> _logger;
    private readonly IMapper _mapper;
    public IngredientController(IngredientRepository ingredientRepository, ILogger<IngredientController> logger, IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{ingredientId:int}")]
    public async Task<IngredientBase> Get(int ingredientId)
    {
        var ingredient = await _ingredientRepository.Get(ingredientId);
        return _mapper.Map<IngredientBase>(ingredient);
    }

    /// <summary>
    /// todo: IngredientBase[] searchMany with ilike
    /// </summary>
    /// <param name="ingredientName"></param>
    /// <returns></returns>
    [HttpPost("search")]
    public async Task<IngredientBase> Search(string ingredientName)
    {
        var ingredient = await _ingredientRepository.Get(ingredientName);
        return _mapper.Map<IngredientBase>(ingredient);
    }

    [HttpPost]
    public async Task<long> Create(IngredientBase ingredient)
    {
        var id = await _ingredientRepository.AddIngredientAsync(ingredient);
        return id;
    }
}
