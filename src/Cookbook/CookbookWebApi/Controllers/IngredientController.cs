using AutoMapper;
using CookbookCommon.DTO;
using CookbookDB.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookbookWebApi.Controllers;

[ApiController]
[Route("cookbook/[controller]/[action]")]
[Authorize]
public class IngredientController(
    IngredientRepository ingredientRepository,
    IMapper mapper): ControllerBase
{
    private readonly IngredientRepository _ingredientRepository = ingredientRepository;

    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IngredientBase> Get([FromQuery] int id)
    {
        var ingredient = await _ingredientRepository.Get(id);
        return _mapper.Map<IngredientBase>(ingredient);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IngredientBase[]> Search([FromQuery] string name)
    {
        var ingredients = await _ingredientRepository.Search(name);
        return ingredients?.Select(_mapper.Map<IngredientBase>).ToArray() ?? [];
    }

    [HttpPost]
    public async Task<long> Create(IngredientCreate ingredient)
    {
        var id = await _ingredientRepository.AddIngredientAsync(ingredient, GetCurrentUserId());
        return id;
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
