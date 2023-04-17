using Drink.Common.DTOs;
using Drink.Common.Models;
using Drink.Database.Entities;

namespace Drink.API.Services
{
    public interface IContentService
    {
        Task<IEnumerable<DishDTO>> MapAndSaveDishesAsync(IEnumerable<Recipe> recipes);
        Task<IEnumerable<DrinkDTO>> MapAndSaveDrinkAsync(IEnumerable<IDrink> iDrinks, DrinkType type);
    }
}
