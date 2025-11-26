using AutoMapper;
using CookbookDB.Models;

namespace CookbookWebApi.Mapping
{
    public class IngredientProfile: Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, CookbookCommon.DTO.IngredientBase>();
        }
    }
}
