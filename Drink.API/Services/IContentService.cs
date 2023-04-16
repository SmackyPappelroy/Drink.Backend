using Drink.Common.DTOs;
using Drink.Common.Models;
using Drink.Database.Entities;

namespace Drink.API.Services
{
    public interface IContentService
    {
        Task<IEnumerable<Dish>> MapAndSaveDishesAsync(IEnumerable<Recipe> recipes);
        Task<IEnumerable<DrinkDTO>> MapAndSaveDrinkAsync(IEnumerable<IDrink> iDrinks, DrinkType type);
    }
}
