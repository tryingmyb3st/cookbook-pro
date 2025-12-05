using AutoMapper;
using CookbookCommon.DTO;
using CookbookDB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]/[action]")]
public class IngredientController(
    IngredientRepository ingredientRepository,
    IMapper mapper): ControllerBase
{
    private readonly IngredientRepository _ingredientRepository = ingredientRepository;

    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IngredientBase> Get([FromQuery] int id)
    {
        var ingredient = await _ingredientRepository.Get(id);
        return _mapper.Map<IngredientBase>(ingredient);
    }

    [HttpGet]
    public async Task<IngredientBase[]> Search([FromQuery] string name)
    {
        var ingredients = await _ingredientRepository.Search(name);
        return ingredients.Select(_mapper.Map<IngredientBase>).ToArray();
    }

    [HttpPost]
    public async Task<long> Create(IngredientCreate ingredient)
    {
        var id = await _ingredientRepository.AddIngredientAsync(ingredient);
        return id;
    }
}
