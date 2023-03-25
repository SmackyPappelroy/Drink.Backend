using Drink.Common.DTOs;
using Drink.Common.Models;

namespace Drink.API.Clients
{
    public static class ContentMapper
    {
        public static DrinkDTO MapToDrink(IDrink drink, DrinkType type)
        {
            return new DrinkDTO
            {
                Name = drink.Name,
                DrinkType = type,
                Description = drink.Description,
                Image = drink.Image
            };
        }
    }
}
